### ViewComponents

#### _Where?_

<!--.element: class="fragment" data-fragment-index="1" -->

- Class goes anywhere
- View goes `Views/Shared/Components/{YourComponentName}/Default.cshtml`

<!--.element: class="fragment" data-fragment-index="1" -->

#### _What?_

<!--.element: class="fragment" data-fragment-index="2" -->

- Inherits from `ViewComponent` and/or name ends with `ViewComponent`
- Method `Process` or `ProcessAsync` by convention
- Returns `IViewComponentResult`
  - `ViewViewComponentResult` <-- not a typo!

<!--.element: class="fragment" data-fragment-index="2" -->

#### _How?_

<!--.element: class="fragment" data-fragment-index="3" -->

- `<vc:{yourComponentName} prop="value"/>`
- All props must be provided or it will fail silently!

<!--.element: class="fragment" data-fragment-index="3" -->
