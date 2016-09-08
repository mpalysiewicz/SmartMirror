registerModule({
    name: "calendar_google",
    initCallback: initCalendarGoogle,
    refreshCallback: refreshCalendarGoogle,
    refreshRate: 600000,
    enabled: true
});

function initCalendarGoogle(){
    gapi.load("client", authenticate);
};

function authenticate() {
    gapi.auth.authorize(
        {
        'client_id': config.calendarGoogle.clientId,
        'scope': config.calendarGoogle.scopes.join(' '),
        'immediate': true
        }, authenticateResult);
}

function authenticateResult(authResult) {
    var authorizeDiv = document.getElementById('authorize-div');
    if (authResult && !authResult.error) {
        authorizeDiv.style.display = 'none';
        gapi.client.load('calendar', 'v3', refreshCalendarGoogle);
    } else {
        authorizeDiv.style.display = 'inline';
    }
}

function handleAuthClick(event) {
    gapi.auth.authorize({
            client_id: config.calendarGoogle.clientId,
            scope: config.calendarGoogle.scopes.join(' '),
            immediate: false}, 
        authenticateResult);
    return false;
}

function refreshCalendarGoogle() {
    var request = gapi.client.calendar.events.list({
        'calendarId': 'primary',
        'timeMin': (new Date()).toISOString(),
        'showDeleted': false,
        'singleEvents': true,
        'maxResults': 10,
        'orderBy': 'startTime'
    });

    request.execute(function(resp) {
        var events = resp.items;
        $("#calendar_google_events").empty();
        $("#calendar_google_events").append("<div class='medium bright'>Upcomming events:</div>");
        for (i = 0; i < events.length && i < 5; i++) {
            var event = events[i];
            var start = event.start.dateTime;
            if (!start) { start = event.start.date; }
            var end = event.end.dateTime;
            if (!end) { end = event.end.date; }
            $("#calendar_google_events").append('<div class="">' + event.summary + '</div>');
            $("#calendar_google_events").append('<div class="xsmall dimmed">' + start + ' - ' + end + '</div>');
        }
    });
}

