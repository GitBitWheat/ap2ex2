//The code is wrapped in a self invoked function in order to avoid conflicts with the global namespace.
$(async function () {
    let r = await fetch("/Users/GetContacts?id=1");
    /*let d = await r.json();
    console.log(d);*/
    console.log(r);
});