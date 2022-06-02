$(async function () {
    let r = await fetch("/Users/GetContacts");
    let d = await r.json();
    console.log(d);
});