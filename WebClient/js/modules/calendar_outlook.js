registerModule({
    name: "calendar_outlook",
    initCallback: initCalendarOutlook,
    refreshCallback: refreshCalendarOutlook,
    refreshRate: 600000,
    enabled: true
});

var oauth = new OAuth2({
    providerId: "Microsoft",
    clientId: config.calendarOutlook.clientId,
    clientSecret: config.calendarOutlook.clientSecret,
    scopes: config.calendarOutlook.scopes,
    redirectUrl: "http://localhost/SmartMirror",
    authorization: "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
    token: "https://login.microsoftonline.com/common/oauth2/v2.0/token",
});

function initCalendarOutlook() {
    oauth.authenticate(function() {
        refreshCalendarOutlook();
    });
};

function refreshCalendarOutlook()
{
    var start = moment().format('YYYY-MM-DDT00:00:00');
    var end = moment().add(1, 'month').format('YYYY-MM-DDT00:00:00');
    $.ajax({
        url: "https://outlook.office.com/api/v2.0/me/calendarview?startDateTime="+start+"&endDateTime="+end,
        headers: oauth.getHeaders(),
        oauth: {
            scopes: {
                request: ["https://outlook.office.com/calendars.read"],
            }
        },
        dataType: 'json',
        success: function(data) {
            $("#calendar_outlook_events").empty();
            $("#calendar_outlook_events").append("<div class='medium bright'>Upcomming events:</div>");
            for (i = 0; i < data.value.length && i < 5; i++) {
                var event = data.value[i];
                var start = event.Start.DateTime.slice(0,19).replace('T', ' ');
                var end = event.End.DateTime.slice(0,19).replace('T', ' ');
                $("#calendar_outlook_events").append('<div class="">' + event.Subject + '</div>');
                $("#calendar_outlook_events").append('<div class="xsmall dimmed">' + start + ' - ' + end + '</div>');
                $("#calendar_outlook_events").append('<div class="xsmall dimmed">' + event.Location.DisplayName + '</div>');
            }
        }
    });
}