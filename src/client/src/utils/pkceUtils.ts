export function generateCodeVerifier(): string {
    const array = new Uint32Array(32);
    window.crypto.getRandomValues(array);

    // Перетворення масиву в бінарний рядок, який не містить проблемних символів для btoa
    const stringified = Array.from(array).map(num => String.fromCharCode(num % 94 + 33)).join('');

    return stringified;
  }

  export async function generateCodeChallenge(verifier: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(verifier);
    const digest = await window.crypto.subtle.digest('SHA-256', data);
    const base64 = btoa(String.fromCharCode(...new Uint8Array(digest)))
      .replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');

    return base64;
  }