registerModule({
    name: "calendar",
    initCallback: initCalendar,
    refreshCallback: refreshCalendar,
    refreshRate: 600000,
    enabled: true
});

var googleFeeder = new GoogleFeeder(dataReady);
var outlookFeeder = new OutlookFeeder(dataReady);

function initCalendar() {
    googleFeeder.init();
    outlookFeeder.init();
}

function refreshCalendar() {
    googleFeeder.getEvents();
}

function dataReady(events) {
}