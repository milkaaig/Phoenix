using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Phoenix.Controllers;
using Phoenix.Data;


namespace Phoenix.Interfaces
{
    public interface IAddFunctions
    {

        Task<IActionResult> GetPosts();
        Task<IActionResult> Index();
        Task<IActionResult> Posting();
        Task<IActionResult> AddPosts();

        Task<IActionResult> SubmitPost(Post post);
        Task<IActionResult> Post(int id);
        Task<IActionResult> AllPosts();
        Task<IActionResult> Delete(int id);
        Task<IActionResult> DeleteAll(int id);
        
        Task<IActionResult> Edit(int id, Post post);
        Task<IActionResult> Edit(int id);
    }
}
