#### What magic words do I need to put in?

##### Instead of this

<!--.element: class="fragment fade-in" data-fragment-index="1" -->

```cs
[HttpGet("question/{id:int}")]
public IActionResult Question(int id)
{
    var question = _dbContext.Questions.FirstOrDefault(q => q.Id == id) ;
    return View(ToViewModel(question));
}
```

<!--.element: class="fragment fade-in" data-fragment-index="1" -->

##### Do this

<!--.element: class="fragment fade-in" data-fragment-index="2" -->

```cs
[HttpGet("question/{id:int}")]
public async Task<IActionResult> Question(int id)
{
    var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == id) ;
    return View(ToViewModel(question));
}
```

<!--.element: class="fragment fade-in" data-fragment-index="2" -->

[good article to read](https://itnext.io/visual-guide-to-csharp-async-await-bc85db77f36a)

<!--.element: class="fragment fade-in" data-fragment-index="3" -->
