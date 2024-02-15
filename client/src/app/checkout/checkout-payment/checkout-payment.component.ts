import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { Basket } from 'src/app/shared/models/basket';
import { Address } from 'src/app/shared/models/user';
import {
  Stripe,
  StripeCardCvcElement,
  StripeCardExpiryElement,
  StripeCardNumberElement,
  loadStripe,
} from '@stripe/stripe-js';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss'],
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardErrors: any;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    loadStripe(
      'pk_test_51OjRIQDbmxdsBdf2A4CSb7iHNHMReht5nbgUQM6ucT1ZTZfVQJ5kRSIykfdaxklS1qeVrBQm3ZiwsevCQSZ7yH4f00sEquqkYl'
    ).then((stripe) => {
      this.stripe = stripe;
      const elements = stripe?.elements();

      if (elements) {
        this.cardNumber = elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on('change', (event) => {
          if (event.error) {
            this.cardErrors = event.error.message;
          } else {
            this.cardErrors = null;
          }
        });

        this.cardExpiry = elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on('change', (event) => {
          if (event.error) {
            this.cardErrors = event.error.message;
          } else {
            this.cardErrors = null;
          }
        });

        this.cardCvc = elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on('change', (event) => {
          if (event.error) {
            this.cardErrors = event.error.message;
          } else {
            this.cardErrors = null;
          }
        });
      }
    });
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    if (!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);
    if (!orderToCreate) return;
    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: (order) => {
        this.toastr.success('Order created successfully');
        this.stripe
          ?.confirmCardPayment(basket.clientSecret!, {
            payment_method: {
              card: this.cardNumber!,
              billing_details: {
                name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value,
              },
            },
          })
          .then((result) => {
            console.log(result);
            if (result.error) {
              this.toastr.error(result.error.message);
            } else {
              if (result.paymentIntent?.status === 'succeeded') {
                this.toastr.success('Payment successful');
                this.basketService.deleteLocalBasket();
                // sending the order object to the success page
                const navigationExtras: NavigationExtras = { state: order };
                this.router.navigate(['checkout/success'], navigationExtras);
              }
            }
          });
      },
      error: (error) => {
        this.toastr.error(error.message);
      },
    });
  }

  //getting info from the form to create an order object
  private getOrderToCreate(basket: Basket) {
    const deliveryMethodId = this.checkoutForm
      ?.get('deliveryForm')
      ?.get('deliveryMethod')?.value;
    const shipToAddress = this.checkoutForm?.get('addressForm')
      ?.value as Address;

    if (!deliveryMethodId || !shipToAddress) return;

    return {
      basketId: basket.id,
      deliveryMethodId,
      shipToAddress,
    };
  }
}