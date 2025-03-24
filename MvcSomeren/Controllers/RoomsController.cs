using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class RoomsController : Controller
{
    private readonly IRoomsRepository _roomRepository;

    public RoomsController(IRoomsRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public IActionResult Index()
    {
        List<Room> rooms = _roomRepository.GetAll();
        return View(rooms);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Room room)
    {
        try
        {
            _roomRepository.Add(room);

            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(room);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(room);
        }
    }

    [HttpGet("Rooms/Edit/{RoomId}")]
    public ActionResult Edit(int? roomId)
    {
        if (roomId == null)
        {
            return NotFound();
        }

        Room? room = _roomRepository.GetById((int)roomId);
        return View(room);
    }

    [HttpPost]
    public ActionResult Edit(Room room)
    {
        try
        {
            _roomRepository.Update(room);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(room);
        }
    }

    [HttpGet("Rooms/Delete/{RoomId}")]
    public ActionResult Delete(int? roomId)
    {
        if (roomId == null)
        {
            return NotFound();
        }

        Room? room = _roomRepository.GetById((int)roomId);
        return View(room);
    }

    [HttpPost]
    public ActionResult Delete(Room room)
    {
        try
        {
            _roomRepository.Delete(room);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(room);
        }
    }
}