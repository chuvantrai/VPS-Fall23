
import { Drawer } from "antd";
import React, { Fragment, useEffect, useState } from "react";
import styles from './Homepage.module.scss'
import classNames from 'classnames/bind';
import HomepageAdmin from "./components/HomepageAdmin";
const cx = classNames.bind(styles);

const HomePage = () => {
  const role = 1;

  const [isDrawerOpen, setDrawerOpen] = useState(true);
  // Initialize and add the map
  let map;
  useEffect(() => {

    initMap();
  }, [])

  async function initMap() {
    // The location of Uluru
    const position = { lat: 20.982570, lng: 105.844949 };
    // Request needed libraries.
    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    // The map, centered at Uluru
    map = new Map(document.getElementById("map"), {
      zoom: 11,
      center: position,
      mapId: "DEMO_MAP_ID",
    });

    // The marker, positioned at Uluru
    const marker = new AdvancedMarkerElement({
      map: map,
      position: position,
      title: "Hóa đơn điện tử M-invoice",

    });
    const infoWindow = new google.maps.InfoWindow({
      content: "Hóa đơn điện tử M-invoice",
      disableAutoPan: true,
    });
    marker.addListener("click", () => {
      infoWindow.open(map, marker);
    });
  }


  return (
    <Fragment>
      {(role == "1") && <HomepageAdmin></HomepageAdmin>}
      {role == "2" && (
        < Fragment>

          <div id="map" style={{
            width: '100vw',
            maxWidth: '100%',
            position: "absolute",
            height: "100%"
          }}></div>

          <Drawer
            title="Danh sách các nhà xe gần bạn"
            placement="left"
            closable={true}
            onClose={() => { setDrawerOpen(!isDrawerOpen) }}
            open={isDrawerOpen}

          >
            <p>Some contents...</p>
            <p>Some contents...</p>
            <p>Some contents...</p>
          </Drawer>
        </Fragment >)}
      {
        role == "3" && < div> Owner
        </div>
      }

    </Fragment >
  )
}
export default HomePage;
