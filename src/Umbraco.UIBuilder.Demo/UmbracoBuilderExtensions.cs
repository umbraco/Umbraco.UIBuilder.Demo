using Umbraco.UIBuilder.Demo.Actions;
using Umbraco.UIBuilder.Demo.DataViews;
using Umbraco.UIBuilder.Demo.Models;
using Umbraco.UIBuilder.Demo.ValueMappers;
using Umbraco.UIBuilder.Extensions;
using Umbraco.UIBuilder.Infrastructure.Configuration.Actions;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;

namespace Umbraco.UIBuilder.Demo
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddUIBuilderDemo(this IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<UdiToIntContentPickerValueMapper>();
            
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
                                        .AddField(c => c.NodeUdi).SetLabel("Node").SetDataType("Content Picker")
                                        .AddField(c => c.Email).SetValidationRegex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+")
                                        .AddField(c => c.Body).SetDataType("Textarea")
                                        .AddField(c => c.Status).SetDataType("Comment Status").SetValueMapper<EnumDropdownValueMapper<CommentStatus>>()
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
