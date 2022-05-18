//The code is wrapped in a self invoked function in order to avoid conflicts with the global namespace.
(function () {
    //Every element in the HTML which needs to be accessed and stored into a constant variable is accessed here. 
    const inputPassword_signup = document.getElementById("inputPassword-signup");
    const passwordVerfication = document.getElementById("verifyPassword");
    const passwordVerificationErrorElem = document.getElementById("passwordVerificationError");

    document.getElementById("signupForm").addEventListener("submit", event => {
        //gets values entered
        const inputPassword_signup_val = inputPassword_signup.value;
        const passwordVerfication_val = passwordVerfication.value;

        //If the password verification does not match, then prevent the form from submition
        if (inputPassword_signup_val != passwordVerfication_val) {
            //Showing the error message
            passwordVerificationErrorElem.style.visibility = "visible";

            //Preventing the form from submition
            event.preventDefault();
            return false;
        }

        //If the password verification does match, then hide the error message
        else
            passwordVerificationErrorElem.style.visibility = "hidden";

        /*//If the user entered a pfp, create a url object of it
          if (!inputPfp.value == "") {
              var inputPfp_val = URL.createObjectURL(inputPfp.files[0]);
          }*/
    });
})();