# Umbraco UI Builder Demo
Demo of Umbraco UI Builder, the back office admin interface builder for Umbraco

## About

This demo consists of a super minimal blog with a comments section on the blog post pages. The comments section consists of a form to submit a comment aswell as a list of all approved comments. In the back end code, there is an example surface controller to handle submitted comments and then the Umbraco UI Builder configuration to display these comments in 3 locations.

### Context App

Comments relevant to a blog post are displayed in context via a context app. In this app you can only see comments that are relevant to the given page.

### Dashboard

On the content section there is a new "Pending Comments" dashboard that lists comments with a `Pending` status. This could be used as a fast window to approve new comments.

### Section

A repositories section is added with a comments collection to allow full management of all comments regardless of which blog post they are associated with.

## How to use

Pull the source for the project and launch the `Umbraco.UIBuilder.Demo.Web` project. You can access the back office at `/umbraco` with admin credentials beign `admin@example.com` / `password1234`.
