registerModule({
    name: "weather",
    initCallback: updateWeather,
    refreshCallback: updateWeather,
    refreshRate: 10000,
	enabled: true
});

var url = 'http://api.openweathermap.org/data/2.5/weather?q=Krakow&appid=a385cfd23c8784ce44b9e30b1a8d48d6&lang=pl&units=metric';

var iconTable = {
	"01d": "wi-day-sunny",
	"02d": "wi-day-cloudy",
	"03d": "wi-cloudy",
	"04d": "wi-cloudy-windy",
	"09d": "wi-showers",
	"10d": "wi-rain",
	"11d": "wi-thunderstorm",
	"13d": "wi-snow",
	"50d": "wi-fog",
	"01n": "wi-night-clear",
	"02n": "wi-night-cloudy",
	"03n": "wi-night-cloudy",
	"04n": "wi-night-cloudy",
	"09n": "wi-night-showers",
	"10n": "wi-night-rain",
	"11n": "wi-night-thunderstorm",
	"13n": "wi-night-snow",
	"50n": "wi-night-alt-cloudy-windy"
};

function updateWeather() {
	$.ajax({
		'async': true,
		'global': false,
		'url': url,
		'dataType': "json",
		'success': function (data) {			
			$("#weather_description").text(data.weather[0].description);
			$("#weather_icon").removeClass().addClass("wi").addClass(iconTable[data.weather[0].icon]);
			$("#weather_temp").text(data.main.temp);
			$("#weather_temp_min").text(data.main.temp_min);
			$("#weather_temp_max").text(data.main.temp_max);
			$("#weather_pressure").text(data.main.pressure);
			$("#weather_humidity").text(data.main.humidity);
		}
	})
};