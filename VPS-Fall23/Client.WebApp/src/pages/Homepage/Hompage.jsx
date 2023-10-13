import { Drawer } from "antd";
import React, { Fragment, useEffect, useState } from "react";
import styles from './Homepage.module.scss'
import classNames from 'classnames/bind';
import HomepageAdmin from "./components/HomepageAdmin";
import DriverHompage from "./components/DriverHompage";
const cx = classNames.bind(styles);

const HomePage = () => {
  const role = 2;



  return (
    <Fragment>
      {(role == "1") && <HomepageAdmin></HomepageAdmin>}
      {role == "2" && <DriverHompage />}
      {
        role == "3" && < div> Owner
        </div>
      }

    </Fragment >
  )
}

export default HomePage;

