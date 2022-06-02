$(document).ready(function () {
    function transfer_message(loggedUserId, contactId, messageContent, contactServer) {
        const body = {
            From: loggedUserId,
            To: contactId,
            Content: messageContent
        };

        $.ajax({
            url: 'http://' + contactServer + '/api/transfer',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(body),
            success: function (data) { console.log(data); }
        });
    }

    var connection = new signalR.HubConnectionBuilder().withUrl("/MessageHub").build();
    connection.start()
    
    connection.on("MessageReceived", function (user, message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        
        const rowDiv = document.createElement("div");
        const colDiv = document.createElement("div");
        const spanD = document.createElement("span");
    
        const messageText = document.createTextNode(msg);
    
        rowDiv.className = "row";
        colDiv.className = "col";
        spanD.className = "othersSpeechBubble";
    
        spanD.appendChild(messageText);
        colDiv.appendChild(spanD);
        rowDiv.appendChild(colDiv);
        document.getElementById("sentChat").appendChild(rowDiv);
    });
    
    document.getElementById("sendTextMessageButton").addEventListener("click", function (event) {
        var user = document.getElementById("userId").innerText;
        var receiverConnectionId = document.getElementById("contactInChatId").innerText;
        var message = document.getElementById("message-to-send").value;
        connection.invoke("SendToUser", user, receiverConnectionId, message).catch(function (err) {
            return console.error(err.toString());
        });
        
        const rowDiv = document.createElement("div");
        const colDiv = document.createElement("div");
        const spanD = document.createElement("span");
    
        const messageText = document.createTextNode(message);
    
        rowDiv.className = "row";
        colDiv.className = "col";
        spanD.className = "usersSpeechBubble";
    
        spanD.appendChild(messageText);
        colDiv.appendChild(spanD);
        rowDiv.appendChild(colDiv);
        document.getElementById("sentChat").appendChild(rowDiv);
        
        event.preventDefault();

        var receiverConnectionServer = document.getElementById("contactInChatServer").innerText;
        transfer_message(user, receiverConnectionId, message, receiverConnectionServer);
    });
});