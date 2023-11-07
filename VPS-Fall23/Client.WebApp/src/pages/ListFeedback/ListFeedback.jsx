/* eslint-disable react-hooks/exhaustive-deps */
import { Table, Button } from 'antd';
import { useEffect, useState } from 'react';

import useFeedbackServices from '@/services/feedbackServices';
import { getAccountJwtModel } from '@/helpers';
import ReplyModal from './components/ReplyModal';

function ListFeedback() {
  const service = useFeedbackServices();
  const account = getAccountJwtModel();

  const columns = [
    {
      title: 'Bãi đỗ xe',
      dataIndex: 'parkingZoneName',
      key: 'parkingZoneName',
      render: (text) => <a>{text}</a>,
    },
    {
      title: 'Người tạo',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Thời gian tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
    },
    {
      title: 'Đánh giá',
      key: 'rate',
      dataIndex: 'rate',
    },
    {
      title: 'Nội dung đánh giá',
      key: 'content',
      dataIndex: 'content',
      width: '20%',
    },
    {
      title: 'Phản hồi',
      key: 'replies',
      dataIndex: 'replies',
      width: '20%',
      render: (_, { replies }) => replies.map((reply) => reply.content),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <>
          {record.replies.length == 0 && (
            <Button
              type="primary"
              onClick={() => {
                handleOpenReplyModal(record.id);
              }}
            >
              Trả lời bình luận
            </Button>
          )}
          {record.replies.length > 0 && <></>}
        </>
      ),
    },
  ];

  const [data, setData] = useState([]);
  const [replyModalOpen, setReplyModalOpen] = useState(false);
  const [feedbackId, setFeedbackId] = useState('');

  const handleOpenReplyModal = (feedbackId) => {
    setFeedbackId(feedbackId);
    setReplyModalOpen(true);
  };

  useEffect(() => {
    service.getFeedbackForOwner(account.UserId).then((res) => {
      setData(res.data.data);
    });
  }, []);

  const handleReplyFeedback = (feedbackId) => {};

  return (
    <>
      <Table className="w-[100%]" columns={columns} dataSource={data} />
      <ReplyModal
        open={replyModalOpen}
        feedbackId={feedbackId}
        onCreate={handleReplyFeedback}
        onCancel={() => {
          setReplyModalOpen(false);
        }}
      />
    </>
  );
}

export default ListFeedback;
