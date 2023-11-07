import { List, Rate } from 'antd';
import { useEffect, useState } from 'react';
import useFeedbackServices from '@/services/feedbackServices.js';
import dayjs from 'dayjs';

const FeedbackList = ({ parkingZoneId }) => {
  const [feedbacks, setFeedbacks] = useState([]);
  const [queryParam, setQueryParam] = useState({ page: 1, pageSize: 5, total: feedbacks.length });
  const feedbackServie = useFeedbackServices();
  useEffect(() => {
    feedbackServie.getByParkingZone(parkingZoneId, queryParam.page, queryParam.pageSize).then((res) => {
      setFeedbacks(res.data.items);
      setQueryParam({ ...queryParam, total: res.data.total });
    });
    return () => {
      setFeedbacks([]);
    };
  }, [parkingZoneId, JSON.stringify(queryParam)]);
  const getFeedbackItem = (item, index) => {
    return (
      <List.Item key={index}>
        <List.Item.Meta
          title={
            <>
              <Rate value={item.rate} /> {dayjs(item.createdAt).format('YYYY-MM-DD HH:mm:ss')}
            </>
          }
          description={<span>{item.content}</span>}
        />
      </List.Item>
    );
  };
  const onPagingChange = (page, pageSize) => {
    setQueryParam({ ...queryParam, pageSize: pageSize, page: page });
  };
  return (
    <List
      dataSource={feedbacks}
      pagination={{
        defaultPageSize: queryParam.pageSize,
        onChange: onPagingChange,
        total: queryParam.total,
        position: 'top',
        size: 'small',
        showLessItems: true,
      }}
      renderItem={getFeedbackItem}
    />
  );
};
export default FeedbackList;
