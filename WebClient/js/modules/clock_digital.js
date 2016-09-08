registerModule({
    name: "clock_digital",
    initCallback: refreshClockDigital,
    refreshCallback: refreshClockDigital,
    refreshRate: 1000,
    enabled: true
});

function refreshClockDigital(){
    var now = moment();

    //$("#clock_title").text(moment().format('dddd').capitalizeFirstLetter());
    $("#clock_date").text(moment().format('dddd, DD MMMM YYYY'));
    //$("#clock_time").text(moment().format('HH:mm:ss'));

    $("#clock_time_hour_minute").text(moment().format('HH:mm'));
    $("#clock_time_second").text(moment().format('ss'));
}