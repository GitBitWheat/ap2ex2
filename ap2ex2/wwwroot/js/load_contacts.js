//The code is wrapped in a self invoked function in order to avoid conflicts with the global namespace.
$(async function () {
    let r = await fetch("/Users/GetContacts");
    let d = await r.json();
    console.log(d);
});