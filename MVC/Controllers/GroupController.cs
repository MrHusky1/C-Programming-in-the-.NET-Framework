using APP.Services;
using Microsoft.AspNetCore.Mvc;
using APP.Domain;
using APP.Models;

namespace MVC.Controllers
{
    public class GroupController : Controller
    {
 
        private readonly GroupService _GroupService;

        public GroupController(GroupService GroupService)
        {
            // Store the injected GroupService instance for use in controller actions.
            _GroupService = GroupService;
        }

        public IActionResult Index()
        {
            // Retrieve all Groups as a list from the service.
            var list = _GroupService.Query<Group>().ToList();

            // Set a message value in ViewBag for the Count key based on the number of Groups found.
            ViewBag.Count = list.Count == 0 ? "No Groups found!" : list.Count == 1 ? "1 Group found." : $"{list.Count} Groups found.";

            // Return the view with the list of Groups.
            return View(list);
        }

        public IActionResult Details(int id)
        {
            // Retrieve the Group with the specified ID.
            var item = _GroupService.Query<Group>().SingleOrDefault(group => group.Id == id);

            // If not found, set a not found message.
            if (item is null)
                ViewBag.Message = "Group not found!";

            // Return the view with the Group details (or null if not found).
            return View(item);
        }

        [HttpGet] // this action method (attribute) specifies that this action only handles HTTP GET requests, no need to write since it's the default
        public IActionResult Create()
        {
            // Return the empty create view.
            return View();
        }

        [HttpPost] // this action method (attribute) specifies that this action only handles HTTP POST requests, default is [HttpGet] if not written
        public IActionResult Create(GroupRequest request)
        {
            // Check if the model state is valid through the data annotations of the request before attempting to create.
            if (ModelState.IsValid)
            {
                // Insert the Group using the service.
                var response = _GroupService.Create(request);

                // If creation was successful, redirect to the Index action to show the updated Group list with operation result message.
                if (response.IsSuccessful)
                {
                    // Set the operation result message in TempData dictionary.
                    TempData["Message"] = response.Message;

                    // Redirect to the Index action to show the updated Group list.
                    return RedirectToAction(nameof(Index));
                }

                // If creation fails, set the error result message in ViewBag.
                ViewBag.Message = response.Message;
            }

            // If model state is invalid or creation failed, return the view with the request model data.
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            // Retrieve the Group data by ID for editing.
            var request = _GroupService.Edit(id);

            // If not found, set a not found message.
            if (request is null)
                ViewBag.Message = "Group not found!";

            // Return the edit view with the Group request model data.
            return View(request);
        }

        [HttpPost] // this action method (attribute) specifies that this action only handles HTTP POST requests, default is [HttpGet] if not written
        public IActionResult Edit(GroupRequest request)
        {
            // Check if the model state is valid through the data annotations of the request before attempting to update.
            if (ModelState.IsValid)
            {
                // Update the Group using the service.
                var response = _GroupService.Update(request);

                // If update was successful, redirect to the details action using the id parameter set as response.Id value.
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                // If update fails, set the error result message in ViewBag.
                ViewBag.Message = response.Message;
            }

            // If model state is invalid or update fails, return the view with the request model data.
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            // Delete the Group with the specified ID.
            var response = _GroupService.Delete(id);

            // Store the result message in TempData to display after redirect.
            TempData["Message"] = response.Message;

            // Redirect to the Index action to show the updated Group list.
            return RedirectToAction(nameof(Index));
        }
    }
}