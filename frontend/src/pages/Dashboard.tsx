import React, { useState } from "react";
import Sidenav from "../components/Sidenav";
import Menu from "../components/Menu";
import Orders from "../components/Orders";
import MakeOrder from "../components/MakeOrder";
import NewFood from "../components/NewFood";

const Dashboard: React.FC = () => {
  const [appState, setAppState] = useState(0);

  return (
    <div className="flex flex-row min-h-screen bg-zinc-950">
      <Sidenav appState={appState} setAppState={setAppState} />
      <div className="flex-grow">
        {appState == 0 ? (
          <Menu />
        ) : appState == 1 ? (
          <Orders />
        ) : appState == 2 ? (
          <MakeOrder />
        ) : appState == 3 ? (
          <NewFood />
        ) : (
          <div>An error has appeard.</div>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
