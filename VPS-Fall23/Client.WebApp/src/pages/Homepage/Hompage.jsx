import HomepageAdmin from "./components/HomepageAdmin";
import { GoogleMap, useJsApiLoader } from "@react-google-maps/api";
import { Drawer } from "antd";
import React, { Fragment, useState } from "react";

import styles from './Homepage.module.scss'
import classNames from 'classnames/bind';

const cx = classNames.bind(styles);
const containerStyle = {
  width: '100vw',
  maxWidth: '100%',
  position: "absolute",
  height: "100%"
};

const center = {
  lat: 20.982533,
  lng: 105.844914
};


const HomePage = () => {
  const role = 1;

  const [isDrawerOpen, setDrawerOpen] = useState(true);

  const { isLoaded } = useJsApiLoader({
    id: 'google-map-script',
    googleMapsApiKey: import.meta.env.VITE_MAP_API_KEY
  })

  const [map, setMap] = React.useState(null)

  const onLoad = React.useCallback(function callback(map) {
    // This is just an example of getting and using the map instance!!! don't just blindly copy!
    const bounds = new window.google.maps.LatLngBounds(center);
    map.fitBounds(bounds);

    setMap(map)
  }, [])

  const onUnmount = React.useCallback(function callback(map) {
    setMap(null)
  }, [])

  return (
    <Fragment>
      {(role == "1") && <HomepageAdmin></HomepageAdmin>}
      {role == "2" && < Fragment>
        <>{isLoaded ? (
          <GoogleMap
            mapContainerStyle={containerStyle}
            center={center}
            zoom={10}
            onLoad={onLoad}
            onUnmount={onUnmount}
          >
            <></>
          </GoogleMap>
        ) : <></>}
          <Drawer
            title="Danh sách các nhà xe gần bạn"
            placement="bottom"
            closable={true}
            onClose={() => { setDrawerOpen(!isDrawerOpen) }}
            open={isDrawerOpen}

          >
            <p>Some contents...</p>
            <p>Some contents...</p>
            <p>Some contents...</p>
          </Drawer></>
      </Fragment>
      }
      {role == "3" && < div> Owner
      </div>
      }

    </Fragment>
  )
}
export default HomePage;
