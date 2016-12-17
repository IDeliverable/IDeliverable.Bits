# IDeliverable.Bits

IDeliverable.Bits is a module for the Orchard CMS that provides various useful pieces of functionality commonly needed for site building. While working on site building projects we often find ourselves needing utility functionality that would be useful beyond the project at hand. This module is the place where we collect such useful bits and bobs, to re-use in future projects and to contribute to anyone else who might find them useful.

## Features

### Bits

Adds miscellaneous useful bits and bobs.

### Navigation Bits

Adds useful bits and bobs related to navigation.

### Recipes Bits

Adds useful bits and bobs related to recipes.

### Layout Bits

Adds useful bits and bobs related to layouts, such as useful elements and shape alternates.

### Projection Bits

Adds useful bits and bobs related to projections, such as useful additional projection filters.

### Content Sockets

Adds the ability to associate content items with one another without the need for taxonomies.

### Content Picker Field Trace-Back

Extends `ContentPickerField` by storing the ID of the content item to which the `ContentPickerField` is attached to on the other end of the relationship (the content item being referred to by the `ContentPickerField`).

For example, let's say your website has a **Session** content item that represents a talk at Orchard Harvest. This **Session** has a content picker field called **Speakers**, where each speaker is a **Speaker** content item. With the default functionality provided by `ContentPickerField`, you can easily get the list of **Speakers** when you have a **Session** since `ContentPickerField` stores a list of **Speaker** IDs. But the other way around is not as easy; you cannot get the list of **Sessions** assigned to a given **Speaker**.

This feature makes that easier by storing the **Session** ID on the **Speaker** content item. This way, when you have a **Speaker** you can easily query for its assigned **Sessions** via the `RelatedByPart`. This behavior is very close to the way taxonomies work. The key difference is that you don't have to turn your content types into `TaxonomyTerm` types.

### App-Relative URL Filter

Adds an `IHtmlFilter` implementation that converts app-relative URLs (starting with `~/`) into relative URLs.

Sometimes your website is hosted at different paths depending on environment. For example, on developer machines the path might be `/OrchardLocal`, in your QA environments the path might be `/Test1` and `/Test2` on the same hostname, while in your production environment there might not be any path (the website is commonly hosted at the root of the hostname).

This presents a challenge when creating hyperlinks. You can't use absolute URLs because hostnames differ depending on environment. You can't use a relative URL (e.g. `/OrchardLocal/about-our-company`) because the path component differs between environments. In Razor views you can use *app-relative URLs* (e.g. `~/about-our-company`) since ASP.NET MVC will replace the `~/` placeholder with the correct subpath between the root of the hostname and where the ASP.NET application root happens to reside.

But you can't use app-relative URLs in content! Sitebuilders and developers commonly try to work around this by introducing automated search and replace in recipes, or even live databases, as part of their deployment pipelines, but this is complex to implement and is brittle and error-prone.

This feature allows you to use app-relative URLs **anywhere you like** including in content you create and edit in the admin UI. The `IHtmlFilter` implementation will find and resolve these app-relative URLs in the rendered response regardless of where they come from.

## Compatibility

This module is compatible with **Orchard version 1.10.x**. The module might also work on older or newer versions of Orchard but this is not guaranteed.

## License

This module is open source and free for use under the permissive [MIT license](https://opensource.org/licenses/MIT), which means you are free to change it, redistribute it and generally use it in whatever way you want.