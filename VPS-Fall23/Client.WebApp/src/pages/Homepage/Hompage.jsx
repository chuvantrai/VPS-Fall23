import HomepageAdmin from "./components/HomepageAdmin";

function Homepage() {
  const role = 1;

  return <h2>
    {(role == "1") && <HomepageAdmin></HomepageAdmin>}
    {role == "2" && < div> Driver
    </div>
    }
    {role == "3" && < div> Owner
    </div>
    }
  </h2 >;
}

export default Homepage;
