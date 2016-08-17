#pragma once

#define SAMPLE_HOLD_LOW_MILLIS 18
#define DEFAULT_MAX_RETRIES 20

namespace TemperatureSensorService
{
	public value struct TemperatureSensorReading
	{
		bool TimedOut;
		bool IsValid;
		double Temperature;
		double Humidity;
		int RetryCount;
	};

	public ref class Dht22Sensor sealed
	{
	public:
		Dht22Sensor(Windows::Devices::Gpio::GpioPin^ pin, Windows::Devices::Gpio::GpioPinDriveMode inputReadMode);
		virtual ~Dht22Sensor();

		Windows::Foundation::IAsyncOperation<TemperatureSensorReading>^ GetReadingAsync();
		Windows::Foundation::IAsyncOperation<TemperatureSensorReading>^ GetReadingAsync(int maxRetries);

	private:
		Windows::Devices::Gpio::GpioPinDriveMode _inputReadMode;
		Windows::Devices::Gpio::GpioPin^ _pin;

		TemperatureSensorReading InternalGetReading();
		TemperatureSensorReading Dht22Sensor::CalculateValues(std::bitset<40> bits);
	};
}
