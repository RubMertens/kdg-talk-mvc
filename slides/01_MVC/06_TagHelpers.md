### TagHelpers

#### _Where?_

- _`_Views/_ViewImports.cshtml`_
- `@addTagHelper *, NameOfMyProject`

#### _What?_

- Inherit van `TagHelper`
- `[ViewContext]` op prop
- Inject `IHtmlHelper`
- Override `Process`