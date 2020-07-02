

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
            chat.client.addChatMessage = function (name, ID, message, time, msgID) {
                $('#ChatContent').append('<div class="chatWarp huyChat position-relative"><div class="UserChat mr-1"><img width="28" height="28" style="border:0" src="../../Content/img/UserImage/' + ID + '.jpg"></div><div><div id="chatUser' + msgID + '" class="text-left"><span class="small">' + name + '</span></div><div id="chatContent' + msgID + '" class="chat" onmouseover="console.log(ShowInfo(' + msgID + '))" onmouseout="console.log(HideInfo(' + msgID + '))">' + message + '</div><div id="chatTime' + msgID + '" class="text-left chatTime" style="display:none"><span class="small">' + time + '</span></div></div></div >');
                $('#chatTime' + msgID).css('left', ($('#chatContent' + msgID).outerWidth() + 35) + 'px'); 
                $('#ChatContent').scrollTop($('#ChatContent').get(0).scrollHeight);


                //$('#ChatContent').append('<div class="chatWarp huyChat"><div class="UserChat mr-1"><img width="28" height="28" style="border:0" src="../../Content/img/UserImage/' + ID + '.jpg" /></div ><div><div><span class="small">' + name + '</span></div><div class="chat">' + message + '</div></div ></div>');
            };
            chat.client.addChatMessageToMe = function (message, time, msgID) {
                $('#ChatContent').append('<div class="myChatWarp huyChat position-relative"><div><div id="chatContent' + msgID + '" class="chat myChat" onmouseover="ShowInfo(' + msgID + ')" onmouseout="HideInfo(' + msgID + ')">' + message + '</div><div id="chatTime' + msgID + '" class="text-left MychatTime" style="display:none"><span class="small">' + time + '</span></div></div ></div >');
                $('#chatTime' + msgID).css('right', ($('#chatContent' + msgID).outerWidth() + 5) + 'px'); 
                $('#ChatContent').scrollTop($('#ChatContent').get(0).scrollHeight);

                //$('#ChatContent').append('<div class="myChatWarp huyChat"><div class="chat myChat">' + message + '</div></div>');
            };
            $('#ChatInput').focus();
            $.connection.hub.start().done(function () {
                $('#ChatContent').scrollTop($('#ChatContent').get(0).scrollHeight);
                console.log($.connection.hub.id);
                $('#ChatInput').keypress(function (e) {
                    if (e.which == 13 && !e.shiftKey) {
                        e.preventDefault();
                        var msg = $('#ChatInput');
                        if (msg.val().length > 0) {
                            console.log(msg.val());
                            var str = '<p>' + msg.val() + '</p>';
                            var matches = msg.val().match(/\bhttps?:\/\/\S+/gi);
                            if (matches != null) {
                                for (var i = 0; i < matches.length; i++) {
                                    console.log(matches[i]);

                                    var url = '<a class="link" href="' + matches[i] + '">' + matches[i] + '</a>';
                                    str = msg.val().replace(matches[i], url);
                                }

                            }
                            console.log(str);
                            chat.server.send(str).done(function (r) {
                                if (r) {
                                    //$('#ChatContent').append('<div class="myChatWarp huyChat"><div class="chat myChat">' + msg.val() + '</div></div>');
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
});
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
function ShowInfo(id) {
    $('#chatTime' + id).css('display', 'block');
}
function HideInfo(id) {
    $('#chatTime' + id).css('display', 'none');

}
