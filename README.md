# Garrett Goodwin — Game Development Portfolio

Portfolio website for Garrett Goodwin, a software engineer and game developer specializing in Unity and C#.

The site includes detailed project case studies for ReapKeep, Upturn, and Light in the Dark, along with selected prototypes and professional experience.

Live site: https://garrettgoodwin.github.io/


## Sharing and navigation

The site includes a GG favicon, homepage sharing image, per-game Open Graph/X metadata, and a root 404.html. Old short project paths (such as /upturn and /light-in-the-dark) lead to the matching /projects/ case study. Known renamed prototypes have additional aliases; the mapping is in assets/legacy-routes.json. The routes are static redirect pages with a visible fallback link. They use browser navigation, not configurable server-side 301 responses.

The old Wix host could not be re-crawled during this update, so these cover the project paths represented in the portfolio and likely renamed equivalents; additional historical slugs can be added when identified. Redirects apply on this hosted site. They cannot redirect traffic arriving at another domain until that domain is pointed to this host. No DNS or custom-domain settings were changed.

All canonical and sharing URLs currently use https://garrettgoodwin.github.io. When switching to a custom domain, update those absolute metadata URLs and the link in the résumé PDF at the same time. Keep image URLs publicly reachable. Social networks may cache older previews.

## Selected code sample

The ReapKeep case study links to code/animation-pause, containing FrozenAnimatorCache.cs, its small IPoolCallbacks.cs interface, an explanation, and setup notes. These are the only ReapKeep C# files included. The full game repository and project assets remain private. The source is unchanged from the supplied project snapshot and has not been compiled or run in Unity as a standalone package during this website update.
