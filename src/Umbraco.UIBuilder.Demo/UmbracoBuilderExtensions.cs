using Umbraco.UIBuilder.Demo.Actions;
using Umbraco.UIBuilder.Demo.DataViews;
using Umbraco.UIBuilder.Demo.Events;
using Umbraco.UIBuilder.Demo.Migrations;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Demo.ValueMappers;
using Umbraco.UIBuilder.Events;
using Umbraco.UIBuilder.Extensions;
using Umbraco.UIBuilder.Infrastructure.Configuration.Actions;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace Umbraco.UIBuilder.Demo
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddUIBuilderDemo(this IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<UdiToIntContentPickerValueMapper>();
            builder.AddNotificationAsyncHandler<UmbracoApplicationStartingNotification, DemoMigrations>();
            builder.AddNotificationHandler<EntityDeletedNotification, DeleteCommentRepliesHandler>();
            builder.AddNotificationHandler<EntityDeletedNotification, DeleteAuthorTopicLinksHandler>();
            builder.AddNotificationHandler<EntitySavingNotification, SetReplyAuthorNameHandler>();
            
            builder.AddUIBuilder(cfg =>
            {

                cfg.AddSectionAfter("media", "Repositories", sectionConfig => sectionConfig
                    .Tree(treeConfig => treeConfig

                        .AddCollection<Comment>(x => x.Id, "Comment", "Comments", "A collection of comments", "icon-chat", "icon-chat", collectionConfig => collectionConfig
                            .SetAlias("comments") 
                            .SetNameProperty(c => c.Name)
                            .SetDateCreatedProperty(c => c.DateCreated)
                            .AddSearchableProperty(c => c.Email)
                            .AddFilterableProperty(c => c.Email)
                            .AddFilterableProperty(c => c.Status)
                            .AddCard("Pending Comments", "icon-chat", x => x.Status == CommentStatus.Pending).SetColor("orange")
                            .AddCard("Total Comments", "icon-chat", x => x.Status == CommentStatus.Pending || x.Status == CommentStatus.Approved || x.Status == CommentStatus.Rejected)
                            .SetDataViewsBuilder<CommentStatusDataViewsBuilder>()
                            .AddAction<ChangeStatusAction>()
                            .AddAction<ExportEntityAction>()
                            .AddAction<ImportEntityAction>()
                            .ListView(listViewConfig => listViewConfig
                                .AddField(c => c.Email)
                                .AddField(c => c.Status)
                            )
                            .Editor(editorConfig => editorConfig
                                .AddTab("General", tabConfig => tabConfig
                                    .AddFieldset("General", fieldsetConfig => fieldsetConfig
                                        .AddField(c => c.NodeUdi).SetLabel("Blog post").SetDataType("Content Picker")
                                        .AddField(c => c.Email).SetValidationRegex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+")
                                        .AddField(c => c.Body).SetDataType("Textarea")
                                    )
                                    // Moderation details sit in a sidebar next to the comment itself
                                    .Sidebar(sidebarConfig => sidebarConfig
                                        .AddFieldset("Moderation", fieldsetConfig => fieldsetConfig
                                            .AddField(c => c.Status).SetDataType("Comment Status").SetValueMapper<EnumDropdownValueMapper<CommentStatus>>()
                                            .AddField(c => c.DateCreated).SetLabel("Posted").MakeReadOnly(d => d.ToString("d MMM yyyy, HH:mm"))
                                        )
                                    )
                                )
                            )
                            .AddChildCollection<CommentReply>(x => x.Id, x => x.CommentId, "Reply", "Replies", "Replies to this comment", "icon-reply-arrow", "icon-reply-arrow", childCollectionConfig => childCollectionConfig
                                .SetNameFormat(r => r.AuthorName ?? "New reply")
                                .SetDateCreatedProperty(r => r.DateCreated)
                                .ListView(listViewConfig => listViewConfig
                                    .AddField(r => r.Body)
                                )
                                .Editor(editorConfig => editorConfig
                                    .AddTab("General", tabConfig => tabConfig
                                        .AddFieldset("General", fieldsetConfig => fieldsetConfig
                                            .AddField(r => r.AuthorId).SetLabel("Author").SetDataType("Author Picker").MakeRequired()
                                            .AddField(r => r.Body).SetDataType("Textarea").MakeRequired()
                                        )
                                    )
                                )
                            )
                        )

                        .AddCollection<Author>(x => x.Id, "Author", "Authors", "The people who write the blog posts", "icon-user", "icon-user", collectionConfig => collectionConfig
                            .SetAlias("authors")
                            .AddRelatedCollection<Author, Topic, AuthorTopic>(x => x.Id, "Topic", "Topics", relationConfig => relationConfig
                                .SetAlias("authorTopics")
                                .SetJunction<AuthorTopic>(x => x.AuthorId, x => x.TopicId)
                            )
                            .SetNameProperty(a => a.Name)
                            .SetDateCreatedProperty(a => a.DateCreated)
                            .AddSearchableProperty(a => a.Email)
                            .ListView(listViewConfig => listViewConfig
                                .AddField(a => a.JobTitle).SetHeading("Job title")
                                .AddField(a => a.Email)
                            )
                            .Editor(editorConfig => editorConfig
                                .AddTab("General", tabConfig => tabConfig
                                    .AddFieldset("General", fieldsetConfig =>
                                    {
                                        fieldsetConfig
                                            .AddField(a => a.JobTitle).SetLabel("Job title")
                                            .AddField(a => a.Email).SetValidationRegex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+")
                                            .AddField(a => a.Bio).SetDataType("Richtext editor");

                                        // Many-to-many: picks topics, stored in the AuthorTopic junction table
                                        fieldsetConfig.AddRelatedCollectionPickerField<Topic>("authorTopics", "Topic Picker", "Writes about");
                                    })
                                )
                            )
                        )

                        .AddCollection<Topic>(x => x.Id, "Topic", "Topics", "Subjects the authors write about", "icon-tag", "icon-tag", collectionConfig => collectionConfig
                            .SetAlias("topics")
                            .SetNameProperty(t => t.Name)
                            .Editor(editorConfig => editorConfig
                                .AddTab("General", tabConfig => tabConfig
                                    .AddFieldset("General", fieldsetConfig => fieldsetConfig
                                        .AddField(t => t.Name).MakeRequired()
                                    )
                                )
                            )
                        )

                    )
                );

                cfg.WithSection(Constants.Applications.Content, withSectionConfig => withSectionConfig

                    .AddDashboard("Pending Comments", dashboardConfig => dashboardConfig
                        .SetCollection<Comment>(x => x.Id, "Comment", "Comments", "A collection of comments", "icon-chat", "icon-chat", collectionConfig => collectionConfig
                            .SetAlias("pendingComments") // All collection aliases must be globally unique
                            .SetNameProperty(c => c.Name)
                            .SetDateCreatedProperty(c => c.DateCreated)
                            .AddSearchableProperty(c => c.Email)
                            .SetFilter(x => x.Status == CommentStatus.Pending)
                            .AddAction<ChangeStatusAction>()
                            .DisableCreate()
                            .ListView(listViewConfig => listViewConfig
                                .AddField(c => c.Email)
                                .AddField(c => c.Status)
                            )
                            .Editor(editorConfig => editorConfig
                                .AddTab("General", tabConfig => tabConfig
                                    .AddFieldset("General", fieldsetConfig => fieldsetConfig
                                        .AddField(c => c.NodeUdi).SetLabel("Node").SetDataType("Content Picker")
                                        .AddField(c => c.Email).SetValidationRegex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+")
                                        .AddField(c => c.Body).SetDataType("Textarea")
                                        .AddField(c => c.Status).SetDataType("Comment Status").SetValueMapper<EnumDropdownValueMapper<CommentStatus>>()
                                    )
                                )
                            )
                        )
                    )

                    .WithTree(Constants.Trees.Content, withTreeConfig => withTreeConfig

                        .AddContextAppAfter("umbContent", "Comments", "icon-chat", ctxAppConfig => ctxAppConfig
                            .SetVisibility(appCtx => appCtx.Source is IContent content && content.ContentType.Alias == "blogPostPage")
                            .AddCollection<Comment>(x => x.Id, x => x.NodeUdi, "Comment", "Comments", "A collection of comments", "icon-chat", "icon-chat", collectionConfig => collectionConfig
                                .SetAlias("relatedComments") // All collection aliases must be globally unique
                                .SetNameProperty(c => c.Name)
                                .SetDateCreatedProperty(c => c.DateCreated)
                                .AddSearchableProperty(c => c.Email)
                                .AddAction<ChangeStatusAction>()
                                .ListView(listViewConfig => listViewConfig
                                    .AddField(c => c.Email)
                                    .AddField(c => c.Status)
                                )
                                .Editor(editorConfig => editorConfig
                                    .AddTab("General", tabConfig => tabConfig
                                        .AddFieldset("General", fieldsetConfig => fieldsetConfig
                                            .AddField(c => c.Email).SetValidationRegex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+")
                                            .AddField(c => c.Body).SetDataType("Textarea")
                                            .AddField(c => c.Status).SetDataType("Comment Status").SetValueMapper<EnumDropdownValueMapper<CommentStatus>>()
                                        )
                                    )
                                )
                            )
                        )

                    )

                );

            });
            
            return builder;
        }
    }
}
