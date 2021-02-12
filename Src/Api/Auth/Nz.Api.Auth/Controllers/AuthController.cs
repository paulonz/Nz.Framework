/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Api.Controllers;
    using Nz.Core.Service.Impl.Auth;

    /// <summary>
    /// Controller responsável pela autenticação de usuários
    /// </summary>
    [AllowAnonymous]
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Serviço de autenticação
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="authService">Serviço de autenticação</param>
        /// <param name="logger">Logger</param>
        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger) : base(logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Autentica o usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        [HttpPost("SignIn")]
        [ProducesResponseType(
            type: typeof(ViewModel.Auth.AuthResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignInAsync(
            [FromBody] ViewModel.Auth.LoginRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(SignIn)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    ViewModel.Auth.AuthResponse authResponse = await _authService.SignInAsync(model).ConfigureAwait(false);

                    if (authResponse != null)
                    {
                        return Ok(authResponse);
                    }

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.NotFound,
                        Message = Validations.Auth_Invalid_Credentials
                    }));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Atualiza o token de autenticação do usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        [HttpPost("RefreshToken")]
        [ProducesResponseType(
            type: typeof(ViewModel.Auth.AuthResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshTokenAsync(
            [FromBody] ViewModel.Auth.RefreshTokenRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(RefreshTokenAsync)}"))
            {
                try
                {
                    ViewModel.Auth.AuthResponse authResponse = await _authService.RefreshTokenAsync(model).ConfigureAwait(false);

                    if (authResponse != null)
                    {
                        return Ok(authResponse);
                    }

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.NotFound,
                        Message = Validations.Auth_Invalid_Token
                    }));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="model">Dados do usuário</param>
        /// <returns>Usuário registrado</returns>
        [HttpPost("Register")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] ViewModel.Auth.RegisterRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(RegisterAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    Core.Model.Impl.Auth.User user = await _authService.RegisterAsync(model).ConfigureAwait(false);

                    return ProcessRequest(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Confirma o registro do usuário
        /// </summary>
        /// <param name="model">Dados de confirmação</param>
        /// <returns>Usuário confirmado</returns>
        [HttpPost("ResendConfirmationCode")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResendConfirmationCodeAsync(
            [FromBody] ViewModel.Auth.ResendConfirmationCodeRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ConfirmRegisterAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    await _authService.ResendConfirmationCodeAsync(model.Email).ConfigureAwait(false);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Confirma o registro do usuário
        /// </summary>
        /// <param name="model">Dados de confirmação</param>
        /// <returns>Usuário confirmado</returns>
        [HttpPost("ConfirmRegister")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmRegisterAsync(
            [FromBody] ViewModel.Auth.ConfirmRegisterRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ConfirmRegisterAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    Core.Model.Impl.Auth.User user = await _authService.ConfirmRegisterAsync(model).ConfigureAwait(false);

                    return ProcessRequest(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Inicia a recuperação de senha de um usuário
        /// </summary>
        /// <param name="model">Dados para recuperação de senha</param>
        /// <returns>Inicio da recuperação de senha</returns>
        [HttpPost("ForgotPassword")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPasswordAsync(
            [FromBody] ViewModel.Auth.ForgotPasswordRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ForgotPasswordAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    await _authService.ForgotPasswordAsync(model).ConfigureAwait(false);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Confirma a recuperação de senha do usuário
        /// </summary>
        /// <param name="model">Dados da recuperação de senha</param>
        /// <returns>Senha recuperada</returns>
        [HttpPost("ConfirmForgotPassword")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmForgotPasswordAsync(
            [FromBody] ViewModel.Auth.ConfirmForgotPasswordRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ConfirmForgotPasswordAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    await _authService.ConfirmForgotPasswordAsync(model).ConfigureAwait(false);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Alteração de senha para um usuário
        /// </summary>
        /// <param name="model">Dados de senha</param>
        /// <returns>Senha alterada</returns>
        [Authorize]
        [HttpPost("ChangePassword")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePasswordAsync(
            [FromBody] ViewModel.Auth.ChangePasswordRequest model)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ChangePasswordAsync)}"))
            {
                try
                {
                    _logger.LogInformation(model.Email);

                    Core.Model.Impl.Auth.User user = await _authService.ChangePasswordAsync(model).ConfigureAwait(false);

                    return ProcessRequest(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }
            }
        }

        /// <summary>
        /// Processamento da request para User
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Sucesso</returns>
        private IActionResult ProcessRequest(Core.Model.Impl.Auth.User user)
        {
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
            {
                ErrorType = ViewModel.ErrorType.ModelValidation,
                Message = Validations.Auth_Invalid_Data
            }));
        }
    }
}
