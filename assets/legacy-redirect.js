// Only trusted, same-origin destinations embedded in the static alias page are used.
const destination = document.querySelector("[data-redirect-target]");
if (destination) {
  const target = new URL(destination.getAttribute("href"), window.location.origin);
  if (target.origin === window.location.origin) {
    target.search = window.location.search;
    if (!target.hash) target.hash = window.location.hash;
    window.location.replace(target.pathname + target.search + target.hash);
  }
}
