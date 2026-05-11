using Bunit;
using Bunit.TestDoubles;
using Ecommerce.Client.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

namespace Ecommerce.Client.Tests.Components;

public class PaginationTests : TestContext
{
    [Fact]
    public void Pagination_RendersCorrectNumberOfPageButtons()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var pageButtons = cut.FindAll("li.page-item");
        pageButtons.Should().HaveCount(5); // prev + 3 pages + next
    }

    [Fact]
    public void Pagination_PreviousButton_IsDisabled_OnFirstPage()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var prev = cut.Find("li.page-item:first-child");
        prev.ClassList.Should().Contain("disabled");
    }

    [Fact]
    public void Pagination_NextButton_IsDisabled_OnLastPage()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 3)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var next = cut.Find("li.page-item:last-child");
        next.ClassList.Should().Contain("disabled");
    }

    [Fact]
    public void Pagination_ActivePage_HasActiveClass()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var pageItems = cut.FindAll("li.page-item");
        pageItems[2].ClassList.Should().Contain("active"); // index 0=prev, 1=page1, 2=page2
    }

    [Fact]
    public async Task Pagination_ClickingNextPage_InvokesCallback()
    {
        int? invokedPage = null;
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, pg => invokedPage = pg)));

        await cut.Find("li.page-item:last-child button").ClickAsync(new());

        invokedPage.Should().Be(2);
    }
}
