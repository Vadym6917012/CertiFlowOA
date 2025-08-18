import logo from "../assets/images/signin-image.jpg";
import { generateCodeChallenge, generateCodeVerifier } from "../utils/pkceUtils";

export default function LoginPage() {
  const clientId = '117858198200-akik17lnvmaakq9vk5vpbdmc1s4qqfh0.apps.googleusercontent.com';
  const redirectUri = 'http://localhost:5173/redirect';

  const redirectToAuth = async () => {
    const verifier = generateCodeVerifier();
    localStorage.setItem('codeVerifier', verifier);
    const challenge = await generateCodeChallenge(verifier);

    const authUrl = `https://accounts.google.com/o/oauth2/v2/auth` +
            `?client_id=${clientId}` +
            `&redirect_uri=${redirectUri}` +
            `&response_type=code` +
            `&scope=openid profile email` +
            `&code_challenge=${challenge}` +
            `&code_challenge_method=S256` +
            `&access_type=offline`;

    window.location.href = authUrl;
  };

  return (
    <div className="login-page main-bg-gray">
      <div className="container">
        <div className="row login-content secondary-bg-white">
          <div className="login-image col-md-6 d-flex justify-content-center align-items-center">
            <figure className="login-figure">
              <img src={logo} alt="logo" />
            </figure>
          </div>
          <div className="login-form col-md-6">
            <h2 className="login-form-header">Sign In</h2>
            <div className="button-wrapper d-flex justify-content-center">
              <button onClick={redirectToAuth} className="login-button btn"><i className="bi bi-google"></i> Увійти</button>
            </div>
            <p className="text-sm text-center text-gray-500 mt-4">
              Використовуйте академічну пошту @oa.edu.ua
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
