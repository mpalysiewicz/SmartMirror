function refreshJob(callback, refreshRate, queueOnly) {
    if(!queueOnly)
        callback();
    setTimeout(function() { refreshJob(callback, refreshRate, false); }, refreshRate);
}

function registerModule(moduleConfig) {
    if(moduleConfig.enabled != true)
        return;
    $(document).ready(function(){
        $('#' + moduleConfig.name).removeClass('notloaded');
        moduleConfig.initCallback();
        refreshJob(moduleConfig.refreshCallback, moduleConfig.refreshRate, true);
    })
}