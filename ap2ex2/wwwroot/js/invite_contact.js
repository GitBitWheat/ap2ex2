$(document).ready(function () {
    function invite_contact(loggedUserId, contactId, contactServer) {
        const body = {
            From: loggedUserId,
            To: contactId,
            Server: 'localhost:5183'
        };

        $.ajax({
            url: 'http://' + contactServer + '/api/invitations',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(body),
            success: function (data) { console.log(data); }
        });
    }

    const inputContactIdElem = document.getElementById("inputContactId");
    const inputContactServerElem = document.getElementById("inputContactServer");
    const loggedUserId = document.getElementById("userId").innerText;

    $('#inviteContactButton').click(function (event) {
        const inputContactId = inputContactIdElem.value;
        const inputContactServer = inputContactServerElem.value;

        invite_contact(loggedUserId, inputContactId, inputContactServer);
    });
});