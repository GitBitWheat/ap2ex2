$(document).ready(function () {
    var token = '';

    function loginReq(userId, password, success, failure) {
        const body = {
            Username: userId,
            Password: password
        };

        $.ajax({
            url: 'http://localhost:5244/api/users/login',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(body),
            statusCode: {
                200: function (data) {
                    token = data;
                    success();
                },
                403: function (data) {
                    failure();
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function signupReq(userId, name, password, success, failure) {
        const body = {
            Id: userId,
            Name: name,
            Password: password
        };

        $.ajax({
            url: 'http://localhost:5244/api/users/signup',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(body),
            statusCode: {
                200: function (data) {
                    success();
                },
                403: function (data) {
                    failure();
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function getContactsReq(success) {
        $.ajax({
            url: 'http://localhost:5244/api/contacts',
            type: 'GET',
            contentType: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            data: {},
            statusCode: {
                200: function (data) {
                    success(data);
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function addContactReq(contactId, contactName, contactServer, success, failure) {
        const body = {
            Id: contactId,
            Name: contactName,
            Server: contactServer
        };

        $.ajax({
            url: 'http://localhost:5244/api/contacts',
            type: 'POST',
            contentType: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            data: JSON.stringify(body),
            statusCode: {
                201: function (data) {
                    success();
                },
                403: function (data) {
                    failure();
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function getMessagesReq(contactId, success) {
        $.ajax({
            url: 'http://localhost:5244/api/contacts/' + contactId + '/messages',
            type: 'GET',
            contentType: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            data: {},
            statusCode: {
                200: function (data) {
                    success(data);
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function sendMessageReq(contactId, messageContent, success) {
        const body = {
            Content: messageContent
        };

        $.ajax({
            url: 'http://localhost:5244/api/contacts/' + contactId + '/messages',
            type: 'POST',
            contentType: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            data: JSON.stringify(body),
            statusCode: {
                201: function (data) {
                    success();
                }
            },
            success: function (data) { console.log(data); }
        });
    }

    function inviteContactReq(loggedUserId, contactId, contactServer) {
        const body = {
            From: loggedUserId,
            To: contactId,
            Server: 'localhost:5244'
        };

        $.ajax({
            url: 'http://' + contactServer + '/api/invitations',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(body),
            success: function (data) { console.log(data); }
        });
    }

    function transferMessageReq(loggedUserId, contactId, messageContent, contactServer) {
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
        
    /**
     * The largest amount of characters the text in the added contacts list
     * of the latest message from a contact can have. If the latest message
     * is longer than this length, then its end is replaced with an ellipsis.
     */
    const MAX_LATEST_MESSAGE_LENGTH = 50;
        
    /**
     * The userId of the user which has logged into the website.
     */
    let loggedUser = null;
    
    /**
     * The Id and server of the selected contact which we can send messages to.
     */
    let sendTo = null;
    let sendToServer = null;
    
    /**
     * A boolean variable which is true when all of the buttons and message input
     * in the send message tab are enabled.
     */
    let areSendMessageTabContentsEnabled = false;
    
    /**
     * A boolean variable which is true when at least one contact has been added.
     */
    let hasAContactBeenAdded = false;
    
    /**
     * A map which maps userIds of added contacts, to the div elements
     * of their latest message text in the added contacts list.
     */
    const latestMessageDivs = new Map();
    
    /**
     * A map which maps userIds of added contacts, to the div elements
     * of the date of their latest message in the added contacts list.
     */
    const latestMessageDateDivs = new Map();
    
    //Every element in the HTML which needs to be accessed and stored into a constant variable is accessed here. 
    const loginBox = document.getElementById("login");
    const signupBox = document.getElementById("signUp");
    const chatBox = document.getElementById("chat");
    
    const inputUsername_login = document.getElementById("inputUsername-login");
    const inputPassword_login = document.getElementById("inputPassword-login");
    const inputUsername_signup = document.getElementById("inputUsername-signup");
    const inputNickname = document.getElementById("inputNickname");
    const inputPassword_signup = document.getElementById("inputPassword-signup");
    const passwordVerfication = document.getElementById("verifyPassword");
    
    const userInfo = document.getElementById("user-info");

    const inputContact = document.getElementById("inputContactId");
    const inputContactName = document.getElementById("inputContactName");
    const inputContactServer = document.getElementById("inputContactServer");

    const chatList = document.getElementById("chatList");
    const closeAddContact = document.getElementById("closeAddContact");
    const sentChat = document.getElementById("sentChat");
    const contactInChat = document.getElementById("contactInChat");
    
    const text_message_to_send = document.getElementById("message-to-send");
    const sendTextMessageButton = document.getElementById("sendTextMessageButton");
    
    //All of the buttons and the message input in the send message tab are disabled by default.
    //They will be enabled once a contact is selected.
    text_message_to_send.disabled = true;
    sendTextMessageButton.disabled = true;
    
    
    //loginButton calls login() when clicked
    document.getElementById("loginButton").addEventListener("click", logIn);
    
    //registerButton calls hideLogin() when clicked (hides the login forms and shows the signup form instead)
    document.getElementById("registerButton").addEventListener("click", hideLogin);
    
    //signUpButton calls signUp() when clicked
    document.getElementById("signUpButton").addEventListener("click", signUp);
    
    //alreadyRegisteredButton calls hideSignup() when clicked (hides the signup forms and shows the login form instead)
    document.getElementById("alreadyRegisteredButton").addEventListener("click", hideSignup);
    
    //addContactButton calls addContact() when clicked
    document.getElementById("addContactButton").addEventListener("click", addContact);

    //inviteContactButton invites a new contact with an invitations request
    document.getElementById("inviteContactButton").addEventListener("click", () => {
        const contactId = inputContact.value;
        const contactServer = inputContactServer.value;

        inviteContactReq(loggedUser, contactId, contactServer);
    });

    
    //sendTextMessageButton sends a new text message when clicked and passes the id of the logged user,
    //unless the user is yet to have typed a text message, in which case nothing will happen.
    sendTextMessageButton.addEventListener("click", () => {
        if (text_message_to_send.value != "") {
            sendMessage(new TextMessage(loggedUser, text_message_to_send.value));
        }
    });

    document.getElementById("logoutButton").addEventListener("click", () => {
        token = '';
        sentChat.innerHTML = "";
        contactInChat.innerHTML = "";
        showLogin();
    });

    showLogin();

    /**
     * A text message. Has the userId of the user who sent it, the content of the message
     * which is a string, and the date in which it has been sent.
     * Can write the text message it represents in the document (meaning, in the chat in
     * the HTML document), and can generate a text for the latest message text in the
     * added contacts list.
     * 
     * @class TextMessage
     */
    class TextMessage {
        /**
         * The constructor of TextMessage.
         * 
         * @param {number} senderId 
         * @param {string} content 
         */
        constructor(senderId, content) {
            this.senderId = senderId;
            this.date = new Date();
            this.content = this.addNewlines(content);
        }
    
        /**
         * Adds a new line after every 100 chars
         * if a space found in first 100 chars it adds a new line to the last space
         * if not it adds a new line at index 100
         * @param {string} str message that's being sent
         * @returns new message with new line every 100 chars
         */
        addNewlines(str) {
            let result = '';
            while (str.length > 0) {
                let lastHundred = str.lastIndexOf(' ', 100);
                if(lastHundred == -1){
                    result += str.substring(0, 100) + '\n';
                    str = str.substring(100);
                }
                else{
                    result += str.substring(0, lastHundred+1) + '\n';
                    str = str.substring(lastHundred+1);
                }
            }
            return result;
        }
    
        /**
         * Writes the text message in the document (meaning, in the chat in the HTML document).
         * 
         * @param {boolean} wasSentByLoggedUser A boolean variable which is true when the message has been sent
         * by the logged user. Used for determining the class of the speech bubble of the message.
         */
        writeMessageInDocument(wasSentByLoggedUser) {
            //Creaing the row and col divs in which the message is
            const rowDiv = document.createElement("div");
            const colDiv = document.createElement("div");
    
            //Creating the span which holds the message
            const spanD = document.createElement("span");
        
            //Creating the text node with the message
            const messageText = document.createTextNode(this.content);
        
            //Setting the classes of the divs and the span
            rowDiv.className = "row";
            colDiv.className = "col";
            spanD.className = wasSentByLoggedUser ? "usersSpeechBubble" : "othersSpeechBubble";
        
            //Appending the elements: the span inside the col div inside the row div inside sentChat
            spanD.appendChild(messageText);
            colDiv.appendChild(spanD);
            rowDiv.appendChild(colDiv);
            sentChat.appendChild(rowDiv);
        
            //Clearing the message input in the send message tab
            text_message_to_send.value = "";
    
            //Scrolling down in the chat
            scrollChat();
        }
    
        /**
         * Generates a string which represents the message, for the latest
         * message text in the added contacts list.
         * 
         * @returns A string which represents the content of the message.
         */
        generateLatestMessageText() {
            return cutLongString(this.content, MAX_LATEST_MESSAGE_LENGTH);
        }
    }
    
    /**
     * Login functions checks wheter the username and password
     * that got entered is found in users map, and triggers
     * show chat function that shoes chat file
     * and getchat that get the chat of a the specific user
     */
    function logIn() {
        const inputUsername_login_val = inputUsername_login.value;
        const inputPassword_login_val = inputPassword_login.value;

        loginReq(inputUsername_login_val, inputPassword_login_val, () => {
            loggedUser = inputUsername_login_val;
            showChat();
            getChat(inputUsername_login_val);
        }, () => {
            alert("Wrong username or password");
        });
    }
    
    /**
     * signup function that allow new users to sign in and adds them to users map
     * and does regular check oover password and username entered
     */
    function signUp() {
        //gets values entered
        const inputUsername_signup_val = inputUsername_signup.value;
        const inputNickname_val = inputNickname.value;
        const inputPassword_signup_val = inputPassword_signup.value;
        const passwordVerfication_val = passwordVerfication.value;
        //check if every filed is not empty
        if (inputUsername_signup_val == "" || inputNickname_val == "" || inputPassword_signup_val == "" || passwordVerfication_val == "") {
            alert("You must fill all fields");
            return;
        }
    
        //does normal check over password entered (length, uppercase, etc...)
        const passwordLen = inputPassword_signup_val.length;
        let isThereADigit = false;
        let isThereAnUppercaseLetter = false;
        let isThereALowercaseLetter = false;
    
        for (let i = 0; i < passwordLen; i++) {
            if ('0' <= inputPassword_signup_val[i] && inputPassword_signup_val[i] <= '9') {
                isThereADigit = true;
            }
            if ('A' <= inputPassword_signup_val[i] && inputPassword_signup_val[i] <= 'Z') {
                isThereAnUppercaseLetter = true;
            }
            if ('a' <= inputPassword_signup_val[i] && inputPassword_signup_val[i] <= 'z') {
                isThereALowercaseLetter = true;
            }
        }
    
        if (!isThereADigit || !isThereAnUppercaseLetter || !isThereALowercaseLetter) {
            alert("A password must contain at least 1 digit, 1 uppercase letter and 1 lowercase letter");
            return;
        }
    
        if (inputPassword_signup_val != passwordVerfication_val) {
            alert("The password verification does not match");
            return;
        }
    

        //Sending request to the api
        signupReq(inputUsername_signup_val, inputNickname_val, inputPassword_signup_val, () => {
            hideSignup();
        }, () => {
            alert("There already exists a user with this username");
        });
    }
    
    /**
     * Add sent message to messages box
     * @param {Message} message message need to be added
     */
    function sendMessage(message) {
        if (message.content == '') {
            return;
        }

        sendMessageReq(sendTo, message.content, () => {
            //writes message in document
            message.writeMessageInDocument(true);
            //updates latest message
            updateLatestMessage(message, sendTo);
            //transfers message
            transferMessageReq(loggedUser, sendTo, message.content, sendToServer);
        });
    }
    
    /**
     * loads contact message to message chat box
     * by using their userId
     * 
     * @param {number} contactId contact's add that messages need to be added
     */
    function loadContactMessages(contactId) {
        if (contactId == sendTo) {
            return;
        }

        getMessagesReq(contactId, messagesList => {
            if (!areSendMessageTabContentsEnabled) {
                enableSendMessageTabContents();
                areSendMessageTabContentsEnabled = true;
            }

            sentChat.innerHTML = "";

            const amountOfMessages = messagesList.length;

            for (let i = 0; i < amountOfMessages; i++) {
                const message = messagesList[i];
                const senderId = message.sent ? loggedUser : contactId;
                const textMessage = new TextMessage(senderId, message.content);
                textMessage.writeMessageInDocument(message.sent);
            }
        });
    }
    
    /**
     * Enables all button when pressed on a contact
     */
    function enableSendMessageTabContents() {
        text_message_to_send.disabled = false;
        sendTextMessageButton.disabled = false;
    }
    
    /**
     * Updates latest message when a new message in sent
     * 
     * @param {Message} latestMessage A message object
     * @param {number} contactId The userId of the contact to which the latest message text
     * will be added next to in the added contacts list
     */
    function updateLatestMessage(latestMessage, contactId) {
        latestMessageDivs.get(contactId).innerHTML = latestMessage.generateLatestMessageText();
        latestMessageDateDivs.get(contactId).innerHTML = latestMessage.date.toUTCString();
    }
    
    /**
     * When pressed on specifec contact it load their
     * info (name and image) to the upper top box
     * 
     * @param {string} nickname nickname of contact
     * @param {URL} profile profile picture
     */
    function loadContactInChat(nickname) {
        const colDiv = document.createElement("div");
        const userImg = document.createElement("img");
        const userNickname = document.createTextNode(nickname);
        userImg.src = "defaultProfile.jpeg";
        userImg.alt = "Avatar";
        userImg.style.width = "60px";
        userImg.style.height = "60px";
        userImg.style.borderRadius = "50%";
        userImg.style.marginRight = "5px";
        colDiv.setAttribute("class", "col", "user");;
        userImg.className = "contact-profile";
    
        contactInChat.innerHTML = "";
    
        colDiv.appendChild(userImg);
        colDiv.appendChild(userNickname);
        contactInChat.appendChild(colDiv);
    }
    
    /**
     * When pressed on add contact button, to add an already existing contact
     * it add then to the left side of the screen in contact area
     */
    function addContact() {
        const inputContact_val = inputContact.value;
        const contactName = inputContactName.value;
        const contactServer = inputContactServer.value;
        if (inputContact_val == "") {
            alert("You must fill the contact's username field");
            return;
        }

        addContactReq(inputContact_val, contactName, contactServer, () => {
            if (!hasAContactBeenAdded) {
                chatList.innerHTML = "";
                hasAContactBeenAdded = true;
            }

            writeContactInDocument(inputContact_val, contactServer, contactName, "", "");
            closeAddContact.click();
        }, () => {
            alert("Can't add this contact");
        });
    }
    
    /**
     * When a message goes over maxLength char
     * it cuts it to first maxLength and adds ... to the end of it
     * 
     * @param {string} text text that needs to be cutted
     * @param {number} maxLength The maximum length of the new text
     * @returns the new text
     */
    function cutLongString(text, maxLength) {
        if (text.length > maxLength) {
            return text.substring(0, maxLength - 3) + "...";
        } else {
            return text;
        }
    }
    
    /**
     * when login succeeds or signup is pressed
     * it hides the login screen
     */
    function hideLogin() {
        event.preventDefault();
        signupBox.style.visibility = "visible";
        loginBox.style.visibility = "hidden";
    }
    
    /**
     * when signup succeeds or login is pressed
     * it hides the signup screen
     */
    function hideSignup() {
        event.preventDefault();
        signupBox.style.visibility = "hidden";
        loginBox.style.visibility = "visible";
    }
    
    /**
     * when login succeeds it shows chat box screen
     */
    function showChat() {
        event.preventDefault();
        signupBox.style.visibility = "hidden";
        loginBox.style.visibility = "hidden";
        chatBox.style.visibility = "visible";
    }

    function showLogin() {
        signupBox.style.visibility = "hidden";
        loginBox.style.visibility = "visible";
        chatBox.style.visibility = "hidden";
    }
    
    /**
     * When a user signin it get the chat that 
     * is releated to him and adds it to chat box
     * with ther name and image
     * @param {string} nickname user nickname that get displayed
     * @param {URL} picture user image to get displayed
     */
    function getChat(nickname) {
        userInfo.innerHTML = ""
        //creats html document to add to the main document
        const userImg = document.createElement("img");
        userImg.src = "defaultProfile.jpeg";
        userImg.style.width = "60px";
        userImg.style.borderRadius = "50%";
        userImg.style.marginRight = "5px";
        const nicknameTextNode = document.createTextNode(nickname);
    
        userInfo.appendChild(userImg);
        userInfo.appendChild(nicknameTextNode);

        getContactsReq(contacts => {
            const contactsLength = contacts.length;

            if (contactsLength > 0) {
                chatList.innerHTML = "";
                hasAContactBeenAdded = true;
            }

            for (let i = 0; i < contactsLength; i++) {
                writeContactInDocument(contacts[i].id, contacts[i].server, contacts[i].name,
                    contacts[i].last, contacts[i].lastdate);
            }
        });
    }
    
    /**
     * Whenever a chat is sent it scroll to the bottom
     * to show the new chat
     */
    function scrollChat(){
        sentChat.scrollTop = sentChat.scrollHeight;
    }

    function writeContactInDocument(contactId, contactServer, contactName, last, lastdate) {
        const latestMessageGeneratedText = last;
        const latestMessageDate = lastdate;

        //creates html file to add to the document
        const listItemOfContact = document.createElement("li");
        listItemOfContact.className = "list-group-item d-flex justify-content-between align-items-start";
        listItemOfContact.addEventListener("click", () => {
            loadContactMessages(contactId);
            loadContactInChat(contactName);
            enableSendMessageTabContents();
            sendTo = contactId;
            sendToServer = contactServer;
        });

        const contactPfpElement = document.createElement("img");
        contactPfpElement.src = "defaultProfile.jpeg";
        contactPfpElement.alt = "Avatar";
        contactPfpElement.className = "contact-profile";

        listItemOfContact.appendChild(contactPfpElement);

        const contactDataDiv = document.createElement("div");
        contactDataDiv.className = "ms-2 me-auto";

        const nicknameDiv = document.createElement("div");
        nicknameDiv.className = "fw-bold";
        nicknameDiv.appendChild(document.createTextNode(contactName));

        contactDataDiv.appendChild(nicknameDiv);

        const latestMessageDiv = document.createElement("div");
        latestMessageDiv.appendChild(document.createTextNode(latestMessageGeneratedText));

        latestMessageDivs.set(contactId, latestMessageDiv);
        contactDataDiv.appendChild(latestMessageDiv);

        const latestMessageDateDiv = document.createElement("div");
        latestMessageDateDiv.className = "latest-message-date";
        latestMessageDateDiv.appendChild(document.createTextNode(latestMessageDate));

        latestMessageDateDivs.set(contactId, latestMessageDateDiv);
        contactDataDiv.appendChild(latestMessageDateDiv);

        listItemOfContact.appendChild(contactDataDiv);
        chatList.appendChild(listItemOfContact);

        inputContact.value = "";
        inputContactName.value = "";
        inputContactServer.value = "";
    }
});
