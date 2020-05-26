

$('#CloseChatBtn').click(function () {
    $.connection.hub.stop();
    $('#ChatArea').collapse('toggle');
    $('#ChatContent').html('');
    $('#StartChat').css('display', 'block');

})
$('#StartChat').click(function () {
    if ($('#ChatArea').is(":hidden")) {
        $('#StartChat').css('display', 'none');
        $('#ChatArea').collapse('toggle');
        console.log(UserID);
        console.log(CourseID);

        if (UserID != 0) {
            var chat = $.connection.chatHub;
            $.connection.hub.qs = { 'UserID': UserID, 'CourseID': CourseID };
            // Create a function that the hub can call back to display messages.
            chat.client.addChatMessage = function (name, ID, message, time) {
                // Add the message to the page.
                $('#ChatContent').append('<div class="chatWarp huyChat"><div class="UserChat mr-1"><img width="28" height="28" style="border:0" src="../../Content/img/UserImage/' + ID + '.jpg" /></div ><div><div><span class="small">' + name + '</span></div><div class="chat">' + message + '</div></div ></div>');
            };
            chat.client.addChatMessageToMe = function (message, time) {
                // Add the message to the page.
                $('#ChatContent').append('<div class="myChatWarp huyChat"><div class="chat myChat">' + message + '</div></div>');
                //$('#ChatContent').append('<div class="chatWarp huyChat"><div class="UserChat mr-1"><img width="28" height="28" style="border:0" src="../../Content/img/UserImage/' + ID + '.jpg" /></div ><div class="chat">' + message + '</div></div >');            
            };
            // Get the user name and store it to prepend to messages.
            // Set initial focus to message input box.
            $('#ChatInput').focus();
            // Start the connection.
            $.connection.hub.start().done(function () {
                $('#ChatContent').scrollTop($('#ChatContent').get(0).scrollHeight);
                console.log($.connection.hub.id);
                $('#ChatInput').keypress(function (e) {
                    if (e.which == 13 && !e.shiftKey) {
                        e.preventDefault();
                        var msg = $('#ChatInput');
                        if (msg.val().length > 0) {
                            console.log(msg.val());
                            chat.server.send(msg.val()).done(function (r) {
                                if (r) {
                                    $('#ChatContent').append('<div class="myChatWarp huyChat"><div class="chat myChat">' + msg.val() + '</div></div>');
                                    msg.val('').focus();
                                    $('#ChatInput').height(26);
                                    $('#ChatContent').scrollTop($('#ChatContent').get(0).scrollHeight);
                                }
                            });
                        }
                    }
                });
            });

        }

    }
    // Reference the auto-generated proxy for the hub.
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
