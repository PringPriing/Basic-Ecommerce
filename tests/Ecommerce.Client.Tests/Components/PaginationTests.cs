using Bunit;
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

        var buttons = cut.FindAll("div.pagination button");
        buttons.Should().HaveCount(5); // prev + 3 pages + next
    }

    [Fact]
    public void Pagination_PreviousButton_IsDisabled_OnFirstPage()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var prev = cut.Find("div.pagination button:first-child");
        prev.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Pagination_NextButton_IsDisabled_OnLastPage()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 3)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var next = cut.Find("div.pagination button:last-child");
        next.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Pagination_ActivePage_HasActiveClass()
    {
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, _ => { })));

        var buttons = cut.FindAll("div.pagination button");
        buttons[2].ClassList.Should().Contain("is-active"); // index 0=prev, 1=page1, 2=page2
    }

    [Fact]
    public async Task Pagination_ClickingNextPage_InvokesCallback()
    {
        int? invokedPage = null;
        var cut = RenderComponent<Pagination>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.TotalPages, 3)
            .Add(p => p.OnPageChanged, EventCallback.Factory.Create<int>(this, pg => invokedPage = pg)));

        await cut.Find("div.pagination button:last-child").ClickAsync(new());

        invokedPage.Should().Be(2);
    }
}
