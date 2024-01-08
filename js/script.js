const toggleButton = document.querySelector('.btn_entrar');
const hiddenDiv = document.getElementById('myDiv');

let isVisible = false;

// Função para mostrar a div
function showDiv() {
    hiddenDiv.style.transform = 'translateY(28vh)';
    isVisible = true;
}

// Função para esconder a div
function hideDiv() {
    hiddenDiv.style.transform = 'translateY(-28vh)';
    isVisible = false;
}

// Adicione um ouvinte de evento de clique ao botão
toggleButton.addEventListener('click', (event) => {
    showDiv();
    event.stopPropagation();
});


var minhaDiv = document.getElementById('myDiv');
var botaoExcecao = document.getElementById('.btn_entrar');

// Adiciona um ouvinte de eventos ao documento
document.addEventListener('click', function (event) {
    // Verifica se o clique ocorreu fora da div
    var clicouFora = !minhaDiv.contains(event.target);

    // Verifica se o clique foi no botão de exceção
    var clicouNoBotao = event.target === botaoExcecao;

    // Executa a lógica com base no clique fora ou no botão de exceção
    if (clicouFora && !clicouNoBotao) {
        hideDiv();
    }
});





const slider = document.querySelector(".slider");
const slides = document.querySelectorAll(".slide");

let currentIndex = 0;
const slideInterval = 5000;

function nextSlide() {
  currentIndex = (currentIndex + 1) % slides.length;
  updateSlider();
}

function updateSlider() {
  const translateXValue = -currentIndex * 100;
  slider.style.transform = `translateX(${translateXValue}%)`;
}

setInterval(nextSlide, slideInterval);



const signupButton = document.getElementById("signup-button"),
  loginButton = document.getElementById("login-button"),
  userForms = document.getElementById("user_options-forms");

/**
 * Add event listener to the "Sign Up" button
 */
signupButton.addEventListener(
  "click",
  () => {
    userForms.classList.remove("bounceRight");
    userForms.classList.add("bounceLeft");
  },
  false
);

/**
 * Add event listener to the "Login" button
 */
loginButton.addEventListener(
  "click",
  () => {
    userForms.classList.remove("bounceLeft");
    userForms.classList.add("bounceRight");
  },
  false
);





