// Scroll indicator
let dotNetHelper;

window.initScrollIndicator = function() {
    window.addEventListener('scroll', updateScrollIndicator);
};

window.registerScrollEvents = function(helper) {
    dotNetHelper = helper;
    window.addEventListener('scroll', updateScrollIndicator);
};
function registerScrollEvents() {
    window.addEventListener('scroll', function () {
        const scrollIndicator = document.querySelector('.scroll-indicator');
        if (scrollIndicator) {
            const scrollPosition = window.scrollY;
            const maxScroll = document.body.scrollHeight - window.innerHeight;
            const scrollPercentage = (scrollPosition / maxScroll) * 100;
            scrollIndicator.style.width = scrollPercentage + '%';
        }
    });
}
window.unregisterScrollEvents = function() {
    window.removeEventListener('scroll', updateScrollIndicator);
    dotNetHelper = null;
};

function updateScrollIndicator() {
    if (dotNetHelper) {
        const scrollPosition = window.scrollY;
        const maxScroll = document.body.scrollHeight - window.innerHeight;
        const scrollPercentage = (scrollPosition / maxScroll) * 100;
        dotNetHelper.invokeMethodAsync('UpdateScrollIndicator', scrollPercentage);
    }
}

// Initialize Lottie player
window.initLottiePlayer = function() {
    const container = document.getElementById('lottie-container');
    if (container) {
        const player = document.createElement('lottie-player');
        player.src = "https://assets3.lottiefiles.com/packages/lf20_qmfs6c3i.json";
        player.background = "transparent";
        player.speed = "1";
        player.style.width = "100%";
        player.style.height = "100%";
        player.hover = true;
        player.loop = true;
        container.appendChild(player);
    }
};

// Toast notifications
window.showToast = function(title, message, type = 'info') {
    // This function can be used from C# code to show toast notifications
    // It will be handled by the Blazor component
    if (window.dotNetHelper) {
        window.dotNetHelper.invokeMethodAsync('ShowToast', message, title, type);
    }
};