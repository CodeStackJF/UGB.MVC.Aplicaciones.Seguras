document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('#frm-login').onsubmit = login;
});

async function login(event)
{
    event.preventDefault();
    let email = document.querySelector('#email').value;
    let password = document.querySelector('#password').value;

    var payload = {
        email: email,
        password: password
    };

    const response = await fetch('/login/authenticate', {
      method: 'POST', // Specifies the request type
      headers: {
        'Content-Type': 'application/json' // Tells the server the data is JSON
      },
      body: JSON.stringify(payload) // Converts JavaScript object to a JSON string
    });

     if (!response.ok) {
      error = await response.json();
      alert(error.message)
      throw new Error(`HTTP error! Status: ${response.status}`);      
    }

    window.location = '/';
}