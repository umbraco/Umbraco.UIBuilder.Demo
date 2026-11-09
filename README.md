# Umbraco UI Builder Demo

Demo of [Umbraco UI Builder](https://umbraco.com/products/add-ons/ui-builder/), the back office admin interface builder for Umbraco.

## About

The demo is a small blog. Visitors read posts and leave comments. Editors moderate those comments, reply to them, and manage the blog's authors, all through back office screens built with UI Builder from plain database tables.

Everything UI Builder does is configured in [`UmbracoBuilderExtensions.cs`](src/Umbraco.UIBuilder.Demo/UmbracoBuilderExtensions.cs). The tables are created and seeded by the migrations in [`Migrations/`](src/Umbraco.UIBuilder.Demo/Migrations).

## What's in the back office

### Repositories section

A custom section, added after Media.

- **Summary dashboard**: the section's landing page shows cards for Comments, Authors and Topics, each with an item count and shortcuts to create or list items.
- **Comments**: every comment, whichever post it's on.
  - Search by email, and filter by email, status or when it was posted (the "Posted" options come from a filter options builder).
  - Cards at the top count comments posted this week (a custom card class), pending comments and all comments.
  - A data view switches between statuses.
  - Actions: **Change Status** (a custom action with its own settings form), plus CSV **Export** and **Import**.
  - The editor has a **Moderation** sidebar with the status and a read-only posted date. The posted date is hidden when creating a comment.
  - The commenter's **email can be entered when creating a comment but is read only afterwards**.
  - A **Replies** tab holds the comment's replies, a child collection. Each reply is written by one of the blog's authors, picked with the Author Picker. Deleting a comment deletes its replies.
- **Moderation Log**: a read-only collection. Every change to a comment's status is recorded with who made it and when. Editors can view entries but not create, change or delete them.
- **Settings** folder:
  - **Authors**: name, job title, email, a rich text bio, and the topics they write about. A Profile tab has one field per editor type: photo (media picker), website (URL picker), role (dropdown), languages (multiple dropdown), years writing (slider), preferred post length (range slider), posts per month (integer with a valid range) and reader rating (decimal with a valid range).
  - **Topics**: linked to authors many-to-many through a related collection (the `AuthorTopic` table), and picked on the author editor.

### Content section

- **Pending Comments dashboard**: a filtered list of comments waiting for approval, with the Change Status action, for quick moderation.
- **Comments context app**: on a blog post, a Comments tab shows only that post's comments.
- **Author Picker**: blog posts pick their author with the UI Builder entity picker. Templates read it as a typed `IEnumerable<Author>`.

### Members section

- **Comments context app**: on a member, a Comments tab lists the comments they posted. Comments made by a logged in member are linked to them.

## What's on the website

- Each post shows its author, plus an author box with their photo, job title, role, bio, website and "Writes about" topics.
- Visitors can submit comments through a surface controller. Comments are saved as Pending.
- Approved comments are listed under the post, with any replies beneath them.

## UI Builder features shown

| Feature | Where |
| --- | --- |
| Custom section, tree and summary dashboard | Repositories section |
| Tree folders | Settings folder |
| List views, search, filters, cards, data views | Comments |
| Filter options builders and custom cards | Comments "Posted" filter and "Posted This Week" card |
| Custom action with a settings form | Change Status |
| CSV export and import actions | Comments |
| Editor tabs, fieldsets and sidebars | Comment editor |
| Conditional read-only fields and field visibility | Comment email and posted date |
| Rich text and content picker fields | Author bio, comment blog post |
| Media and URL pickers, dropdowns, sliders, number ranges | Author Profile tab |
| Child collections | Comment replies |
| Related collections (many-to-many) | Author topics |
| Read-only collections | Moderation Log |
| Entity picker property editor | Blog post author, reply author |
| Dashboards and context apps | Content and Members sections |
| Repository API from views and actions | Blog post template, Change Status |
| Entity save and delete notifications | `Events/` |

## How to use

Pull the source for the project and launch the `Umbraco.UIBuilder.Demo.Web` project. You can access the back office at `/umbraco` with admin credentials being `admin@example.com` / `password1234`. The site uses a SQLite database that is included in the repository, so no setup is needed.

## Versions

Each supported Umbraco UI Builder major has its own branch:

| Branch | Umbraco UI Builder |
| --- | --- |
| `main` | Latest stable major |
| `support/{major}.x` | Older majors still in support (e.g. `support/17.x`) |
| `release/{version}` | The next major, on prerelease packages |
