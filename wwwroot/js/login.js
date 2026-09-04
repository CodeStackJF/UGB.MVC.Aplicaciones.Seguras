document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('#btn-login').onclick = login;
});

function login(event)
{
    console.log(3);
    //event.preventDefault();
    let email = document.querySelector('#email').value;
    let password = document.querySelector('#password').value;

    var payload = {
        email: email,
        password: password
    };

    console.log(payload);

    const response = fetch('/login/authenticate', {
      method: 'POST', // Specifies the request type
      headers: {
        'Content-Type': 'application/json' // Tells the server the data is JSON
      },
      body: JSON.stringify(payload) // Converts JavaScript object to a JSON string
    });

     if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const data = response.json(); 
    console.log(data);
}