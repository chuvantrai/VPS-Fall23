import { GoogleMap, useJsApiLoader } from "@react-google-maps/api";
import { Drawer, Layout } from "antd";
import { Content } from "antd/es/layout/layout";
import React, { useState } from "react";
import Footer from "@/layouts/components/Footer";
import styles from './Homepage.module.scss'
import classNames from 'classnames/bind';


const cx = classNames.bind(styles);
const containerStyle = {
  width: '100vw',
  height: '50vh',
  maxWidth: '100%',
  maxHeight: '100%'
};

const center = {
  lat: -3.745,
  lng: -38.523
};


const HomePage = () => {

  const [isDrawerOpen, setDrawerOpen] = useState(false);

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
    <Layout
      className={cx('wrapper')}
    >
      <div className={cx('content')}>
        <Content>



          {isLoaded ? (
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
          </Drawer>

        </Content>
        <Footer></Footer>
      </div>
    </Layout>

  )
}
export default HomePage;
