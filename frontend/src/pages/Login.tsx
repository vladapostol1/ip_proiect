import React, { useState } from "react";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const hanndleSubmit = (e) => {
    e.preventDefault();
    //Handle login logic
    console.log("Username:", username);
    console.log("Password:", password);
  };
  return (
    <div className="bg-zinc-900 w-screen h-screen">
      <form
        onSubmit={hanndleSubmit}
        className="p-4 bg-white shadow-sm rounded-md text-black"
      >
        <h2>Login</h2>
        <div className="flex">
          <label>Email:</label>
          <input
            type="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default Login;
