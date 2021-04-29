using Api;
using Moq;
using NUnit.Framework;

namespace ApiTests
{
    public class AuthenticationControllerTests
    {
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Mock<IIdentityProviderService> _identityProviderServiceMock;

        [SetUp]
        public void SetUp()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationServiceMock.Setup(x => 
                x.ValidateAuthorization(It.IsAny<string>())).Returns(true);
            _identityProviderServiceMock = new Mock<IIdentityProviderService>();
        }
        
        [Test]
        public void IsAuthenticated_ReturnsTrue_WhenIsAdmin()
        {
            var adminIdentity = new Identity {Username = "Admin", IdentityType = IdentityType.Admin};
            _identityProviderServiceMock.Setup(x => x.GetIdentity()).Returns(adminIdentity);
            var authenticationController = new AuthenticationController(_identityProviderServiceMock.Object, 
                _authenticationServiceMock.Object);
            
            var result = authenticationController.IsAuthenticated();
            
            _authenticationServiceMock.Verify(x => 
                x.ValidateAuthorization("Admin"), Times.Once);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsAuthenticated_ReturnsFalse_WhenIsViewer()
        {
            var viewerIdentity = new Identity { Username = "Viewer", IdentityType = IdentityType.Viewer };
            _identityProviderServiceMock.Setup(x => x.GetIdentity()).Returns(viewerIdentity);
            var authenticationController = new AuthenticationController(_identityProviderServiceMock.Object, 
                _authenticationServiceMock.Object);
            var result = authenticationController.IsAuthenticated();
            
            _authenticationServiceMock.Verify(x => 
                x.ValidateAuthorization(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsAuthenticated_ReturnsTrue_WhenIsEditor()
        {
            var editorIdentity = new Identity { Username = "Editor", IdentityType = IdentityType.Editor };
            _identityProviderServiceMock.Setup(x => x.GetIdentity()).Returns(editorIdentity);
            var authenticationController = new AuthenticationController(_identityProviderServiceMock.Object, 
                _authenticationServiceMock.Object);
            var result = authenticationController.IsAuthenticated();
            
            _authenticationServiceMock.Verify(x => 
                x.ValidateAuthorization("Editor"), Times.Once);
            Assert.AreEqual(true, result);
        }
    }
}