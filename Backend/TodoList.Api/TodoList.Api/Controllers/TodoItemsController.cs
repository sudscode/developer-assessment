using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Mappers;
using TodoList.Api.Requests;
using TodoList.Api.Responses;
using TodoList.Application;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoListRepository _todoListRepository;
        private CancellationToken _cancel;
        private readonly IMapper _mapper;

        public TodoItemsController(ILogger<TodoItemsController> logger,
                                    ITodoListRepository todoListRepository,
                                    IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _todoListRepository = todoListRepository ?? throw new ArgumentNullException(nameof(todoListRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cancel = new CancellationTokenSource().Token;
        }

        // GET: api/TodoItems
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoItems()
        {
            //var results = await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
            var results = await _todoListRepository.GetTodoItemsAsync();
            return Ok(_mapper.MapDomainToResponse(results));
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            //var result = await _context.TodoItems.FindAsync(id);
            var result = await _todoListRepository.GetTodoItemByIdAsync(id, _cancel);


            return result == null ? NotFound() : Ok(_mapper.MapDomainToResponse(result));
        }

        //PUT: api/TodoItems/... 
        [HttpPut("{id}")]

        [ProducesResponseType(typeof(TodoItemResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItemUpdateRequest updateRequest)
        {
            if (!_todoListRepository.TodoItemIdExists(id))
            {
                return NotFound();
            }
            var todoItem = _mapper.MapUpdateRequestToDomain(updateRequest, id);

            var result = await _todoListRepository.UpdateTodoItemAsync(todoItem, _cancel);

            // could use Result object e,g result.isFail() with errormessages

            return result > 0 ? Accepted() : Conflict("Update unsuccessful");
        }

        // POST: api/TodoItems 
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostTodoItem(TodoItemRequest todoItemRequest)
        {
            // validation attribute will handle this

            //if (string.IsNullOrEmpty(todoItem?.Description))
            //{
            //    return BadRequest("Description is required");
            //}

            //Combining the validations and not giving away too much of system internals
            //Conflict response seems to be more apt

            var existingTodoItem = await _todoListRepository.GetTodoItemByIdAsync(todoItemRequest.Id, _cancel);
            if (existingTodoItem != null || _todoListRepository.DescriptionAlreadyInUse(todoItemRequest.Description))
            {
                return Conflict();
            }

            await _todoListRepository.AddTodoItemAsync(_mapper.MapRequestToDomain(todoItemRequest), _cancel);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItemRequest.Id }, todoItemRequest);
        }
    }
}
