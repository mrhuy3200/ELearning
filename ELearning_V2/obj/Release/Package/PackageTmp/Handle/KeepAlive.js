//function heartbeat() {
//    $.get(
//        "/SessionHeartbeat.ashx",
//        null,
//        function(data) {
//            beatHeart(2);
//            setHeartbeat();
//        },
//        "json"
//    );
//}

var intervalH;
function beatHeart(times) {
    intervalH = setInterval(function () {
        $(".heartbeat").fadeIn(1000, function () {
            $(".heartbeat").fadeOut(1000);
        });
    }, 2000); // beat every second

    // after n times, let's clear the interval (adding 100ms of safe gap)
    //setTimeout(function () { clearInterval(interval); }, (1000 * times) + 100);
}


SessionUpdater = (function () {
    var clientMovedSinceLastTimeout = false;
    var keepSessionAliveUrl = null;
    var timeout = 4 * 1000 * 60 + 30000; // 5 minutes

    function setupSessionUpdater(actionUrl) {
        // store local value
        keepSessionAliveUrl = actionUrl;
        // setup handlers
        listenForChanges();
        // start timeout - it'll run after n minutes
        checkToKeepSessionAlive();
    }

    function listenForChanges() {
        $("body").one("mousemove keydown", function () {
            clientMovedSinceLastTimeout = true;
        });
    }


    // fires every n minutes - if there's been movement ping server and restart timer
    function checkToKeepSessionAlive() {
        setTimeout(function () { keepSessionAlive(); }, timeout);
    }

    function keepSessionAlive() {
        // if we've had any movement since last run, ping the server
        if (clientMovedSinceLastTimeout && keepSessionAliveUrl != null) {
            console.log("Keep alive")
            $.ajax({
                type: "POST",
                url: keepSessionAliveUrl,
                success: function (data) {
                    // reset movement flag
                    clientMovedSinceLastTimeout = false;
                    // start listening for changes again
                    listenForChanges();
                    // restart timeout to check again in n minutes
                    checkToKeepSessionAlive();


                },
                error: function (data) {
                    console.log("Error posting to " & keepSessionAliveUrl);
                }
            });
        }
        else {
            clearInterval(intervalH);
        }
    }

    // export setup method
    return {
        Setup: setupSessionUpdater
    };

})();