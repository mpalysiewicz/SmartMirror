#include <avr/io.h>
#include <avr/pgmspace.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <stdlib.h>
#include "usb_keyboard.h"

#define LED_CONFIG	(DDRD |= (1<<6))
#define LED_OFF		(PORTD &= ~(1<<6))
#define LED_ON		(PORTD |= (1<<6))
#define CPU_PRESCALE(n)	(CLKPR = 0x80, CLKPR = (n))

int main(void)
{
	// set for 16 MHz clock
	CPU_PRESCALE(0);

	// Configure all port B and port D pins as inputs with pullup resistors.
	// See the "Using I/O Pins" page for details.
	// http://www.pjrc.com/teensy/pins.html
	DDRD = 0x01;
	DDRB = 0x00;
	PORTB = 0x0;
	PORTD = 0xff;

	// Initialize the USB, and then wait for the host to set configuration.
	// If the Teensy is powered without a PkXC connected to the USB port,
	// this will wait forever.
	usb_init();
	while (!usb_configured()) /* wait */ ;

	// Wait an extra seconds for the PC's operating system to load drivers
	// and do whatever it does to actually be ready for input
	 _delay_ms(2000);

	while(1)
	{
		if (!(PINB & (1<<2)) )
		{
			usb_keyboard_press(KEY_PERIOD, 0);
			LED_ON;
		} 
		else 
		{
			LED_OFF;
		}
		_delay_ms(500);		
	}

}