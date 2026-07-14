(function () {
    "use strict";

    if (!("IntersectionObserver" in window)) return;
    if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) return;

    var selector = [
        ".v-brand",
        ".v-featured__header",
        ".v-grid",
        ".v-coll-grid",
        ".v-manifesto__inner",
        ".v-newsletter__inner",
        ".v-footer__inner",
        ".v-detail__media",
        ".v-detail__info",
        ".v-coll-belt",
        ".v-reviews__header",
        ".v-review-form",
        ".v-review-list",
        ".v-cart-list",
        ".v-cart-summary",
        ".v-cart-actions",
        ".v-checkout-summary",
        ".v-address-list",
        ".v-orders-list .v-order",
        ".v-auth__box"
    ].join(",");

    var targets = document.querySelectorAll(selector);
    if (!targets.length) return;

    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.12, rootMargin: "0px 0px -40px 0px" });

    targets.forEach(function (el) {
        el.classList.add("v-reveal");
        observer.observe(el);
    });
})();
